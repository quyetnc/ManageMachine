import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MachineService, Machine } from 'src/app/core/services/machine.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-machine-detail',
  templateUrl: './machine-detail.component.html',
  styleUrls: ['./machine-detail.component.scss']
})
export class MachineDetailComponent implements OnInit {
  machine: Machine | null = null;
  loading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private machineService: MachineService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    const idOrGuid = this.route.snapshot.paramMap.get('id');
    if (idOrGuid) {
      if (/^\d+$/.test(idOrGuid)) {
        // It's a numeric ID (from list navigation)
        this.loadDetail(Number(idOrGuid));
      } else {
        // It's a GUID (from QR scan)
        this.loadByGuid(idOrGuid);
      }
    }
  }

  loadByGuid(guid: string) {
    this.machineService.getMachines().subscribe({
      next: (machines) => {
        const match = machines.find(m => m.qrCodeData === guid);
        if (match) {
          // If list doesn't return full details (params), fetch detail
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
}