import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { RequestService, MachineTransferRequest, RequestStatus, RequestType } from 'src/app/core/services/request.service';

@Component({
    selector: 'app-user-request-list',
    templateUrl: './user-request-list.component.html',
    styleUrls: ['./user-request-list.component.scss']
})
export class UserRequestListComponent implements OnInit {
    requests: MachineTransferRequest[] = [];
    loading = true;
    RequestStatus = RequestStatus;
    RequestType = RequestType;

    constructor(
        private requestService: RequestService,
        private router: Router,
        private snackBar: MatSnackBar
    ) { }

    ngOnInit(): void {
        this.loadRequests();
    }

    loadRequests() {
        this.loading = true;
        this.requestService.getMyRequests().subscribe({
            next: (data) => {
                this.requests = data;
                this.loading = false;
            },
            error: (err) => {
                this.loading = false;
                this.snackBar.open('Failed to load requests', 'Close', { duration: 3000 });
            }
        });
    }

    goBack() {
        this.router.navigate(['/public/profile']);
    }

    cancelRequest(request: MachineTransferRequest) {
        if (confirm('Are you sure you want to cancel this request?')) {
            this.requestService.cancelRequest(request.id).subscribe({
                next: () => {
                    this.snackBar.open('Request cancelled', 'Close', { duration: 3000 });
                    this.loadRequests();
                },
                error: (err) => {
                    this.snackBar.open('Failed to cancel request', 'Close', { duration: 3000 });
                }
            });
        }
    }

    getStatusClass(status: RequestStatus): string {
        switch (status) {
            case RequestStatus.Approved: return 'status-approved';
            case RequestStatus.Rejected: return 'status-rejected';
            case RequestStatus.Pending: return 'status-pending';
            default: return '';
        }
    }

    getStatusLabel(status: RequestStatus): string {
        switch (status) {
            case RequestStatus.Approved: return 'Approved';
            case RequestStatus.Rejected: return 'Rejected';
            case RequestStatus.Pending: return 'Pending';
            default: return 'Unknown';
        }
    }

    getTypeLabel(type: RequestType): string {
        switch (type) {
            case RequestType.Borrow: return 'Borrow';
            case RequestType.Repair: return 'Repair';
            default: return 'Unknown';
        }
    }
}
