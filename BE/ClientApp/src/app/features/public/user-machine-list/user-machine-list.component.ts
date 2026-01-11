import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Machine, MachineService } from 'src/app/core/services/machine.service';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
    selector: 'app-user-machine-list',
    templateUrl: './user-machine-list.component.html',
    styleUrls: ['./user-machine-list.component.scss']
})
export class UserMachineListComponent implements OnInit {
    machines: Machine[] = [];
    filteredMachines: Machine[] = [];
    typeId: number = 0;
    loading = false;
    typeName: string = '';

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private machineService: MachineService,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        const idParam = this.route.snapshot.paramMap.get('id');
        this.loading = true;

        // Check if we are in 'my-machines' route
        const isMyMachines = this.router.url.includes('my-machines');

        // ALWAYS load only my machines
        this.machineService.getMyMachines().subscribe({
            next: (data) => {
                this.machines = data;

                if (isMyMachines) {
                    this.typeName = 'My Assigned Machines';
                    this.filteredMachines = data;
                } else {
                    // Filter by type from my assigned machines
                    this.typeId = idParam ? Number(idParam) : 0;
                    this.filteredMachines = this.machines.filter(m => m.machineTypeId === this.typeId);

                    if (this.filteredMachines.length > 0) {
                        this.typeName = this.filteredMachines[0].machineType?.name || 'Machines';
                    } else {
                        // Try to find the type name from the typeId (optional, but nice to have)
                        // For now just show generic
                        this.typeName = 'Machines';
                    }
                }

                this.loading = false;
            },
            error: (err) => {
                console.error('Failed to load machines', err);
                this.loading = false;
            }
        });
    }

    openMachine(machineId: number) {
        this.router.navigate(['/public/machines', machineId]);
    }

    goBack() {
        this.router.navigate(['/public']);
    }

    logout() {
        this.authService.logout();
    }
}
