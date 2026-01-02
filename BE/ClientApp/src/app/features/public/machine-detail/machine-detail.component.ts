import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
    private machineService: MachineService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    const guid = this.route.snapshot.paramMap.get('id'); // Assuming this param is the QR GUID
    if (guid) {
      // Implement GetByQRCode in Service
      // For now, since we don't have the endpoint, I'll assume we pass ID
      // Or I need to add the endpoint to backend.
      // Let's check if the user passes ID or GUID. 
      // In ScanComponent, I passed 'value' which is GUID.
      // SO I need a way to look up by GUID.
      // Temporary hack: Fetch all machines and find one with GUID (not efficient but works for small app)
      this.loadByGuid(guid);
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
}
