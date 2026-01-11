import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MachineService, Machine } from 'src/app/core/services/machine.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from 'src/app/core/services/auth.service';
import { RequestService, CreateMachineTransferRequestDto } from 'src/app/core/services/request.service';
import { MatDialog } from '@angular/material/dialog';
import { TransferDialogComponent } from './transfer-dialog.component';

@Component({
  selector: 'app-machine-detail',
  templateUrl: './machine-detail.component.html',
  styleUrls: ['./machine-detail.component.scss']
})
export class MachineDetailComponent implements OnInit {
  machine: Machine | null = null;
  loading = true;
  error: string | null = null;
  currentUserId: number | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private machineService: MachineService,
    private snackBar: MatSnackBar,
    private authService: AuthService,
    private requestService: RequestService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.currentUserId = this.authService.currentUserValue?.id || null;
    const idOrGuid = this.route.snapshot.paramMap.get('id');
    if (idOrGuid) {
      if (/^\d+$/.test(idOrGuid)) {
        this.loadDetail(Number(idOrGuid));
      } else {
        this.loadByGuid(idOrGuid);
      }
    }
  }

  // ... (Keep existing methods loadByGuid and loadDetail)
  loadByGuid(guid: string) {
    this.machineService.getMachines().subscribe({
      next: (machines) => {
        const match = machines.find(m => m.qrCodeData === guid);
        if (match) {
          this.loadDetail(match.id);
        } else {
          this.loading = false;
          this.error = 'Machine not found with this QR Code';
        }
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Failed to scan database';
      }
    });
  }

  loadDetail(id: number) {
    this.machineService.getMachine(id).subscribe({
      next: (data) => {
        this.machine = data;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Failed to load machine details';
      }
    });
  }

  goBack() {
    this.router.navigate(['/public']);
  }

  openTransferDialog() {
    if (!this.machine) return;

    const dialogRef = this.dialog.open(TransferDialogComponent, {
      width: '400px',
      data: { ownerId: this.machine.userId }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const dto: CreateMachineTransferRequestDto = {
          machineId: this.machine!.id,
          toUserId: result.toUserId,
          requestType: result.requestType,
          reason: result.reason
        };

        this.requestService.createRequest(dto).subscribe({
          next: () => {
            this.snackBar.open('Transfer request sent successfully!', 'Close', { duration: 3000 });
          },
          error: (err) => {
            this.snackBar.open('Failed to send request', 'Close', { duration: 3000 });
            console.error(err);
          }
        });
      }
    });
  }

  returnMachine() {
    if (!this.machine) return;
    if (confirm('Are you sure you want to return this machine?')) {
      this.machineService.returnMachine(this.machine.id).subscribe({
        next: () => {
          this.snackBar.open('Machine returned successfully', 'Close', { duration: 3000 });
          this.loadDetail(this.machine!.id); // Reload to update status
        },
        error: (err) => {
          this.snackBar.open('Failed to return machine', 'Close', { duration: 3000 });
        }
      });
    }
  }

  cancelRequest() {
    if (!this.machine || !this.machine.pendingTransferRequestId) return;
    if (confirm('Are you sure you want to cancel the pending transfer request?')) {
      this.requestService.cancelRequest(this.machine.pendingTransferRequestId).subscribe({
        next: () => {
          this.snackBar.open('Request cancelled successfully', 'Close', { duration: 3000 });
          this.loadDetail(this.machine!.id);
        },
        error: (err) => {
          this.snackBar.open('Failed to cancel request', 'Close', { duration: 3000 });
        }
      });
    }
  }
}