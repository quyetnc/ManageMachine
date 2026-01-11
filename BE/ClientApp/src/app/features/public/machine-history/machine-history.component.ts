import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MachineService, Machine } from 'src/app/core/services/machine.service';

@Component({
    selector: 'app-machine-history',
    templateUrl: './machine-history.component.html',
    styleUrls: ['./machine-history.component.scss']
})
export class MachineHistoryComponent implements OnInit {
    machineId: number = 0;
    machine: Machine | null = null;
    history: any[] = [];
    loading = true;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private machineService: MachineService
    ) { }

    ngOnInit(): void {
        const idParam = this.route.snapshot.paramMap.get('id');
        if (idParam && /^\d+$/.test(idParam)) {
            this.machineId = Number(idParam);
            this.loadData();
        } else {
            this.loading = false; // Or handle error
        }
    }

    loadData() {
        this.loading = true;
        // Load Machine Info
        this.machineService.getMachine(this.machineId).subscribe({
            next: (m) => this.machine = m,
            error: () => console.error("Failed to load machine info")
        });

        // Load History
        this.machineService.getHistory(this.machineId).subscribe({
            next: (data) => {
                this.history = data;
                this.loading = false;
            },
            error: (err) => {
                console.error('Failed to load history', err);
                this.loading = false;
            }
        });
    }

    getHistoryClass(record: any): string {
        if (record.requestType === 2) return 'return'; // Return
        return 'transfer';
    }

    goBack() {
        // If Admin, maybe go back to admin list? 
        // For now, default to public detail or simple location back
        // Let's go to machine detail
        this.router.navigate(['/public/machines', this.machineId]);
    }
}
