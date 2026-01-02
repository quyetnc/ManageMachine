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
        this.typeId = idParam ? Number(idParam) : 0;
        this.loading = true;

        this.machineService.getMachines().subscribe({
            next: (data) => {
                this.machines = data;
                this.filteredMachines = this.machines.filter(m => m.machineTypeId === this.typeId);

                if (this.filteredMachines.length > 0) {
                    this.typeName = this.filteredMachines[0].machineType?.name || 'Machines';
                } else {
                    this.typeName = 'Machines';
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
