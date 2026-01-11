import { Component, OnInit } from '@angular/core';
import { RequestService, MachineTransferRequest } from 'src/app/core/services/request.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'app-request-list',
    templateUrl: './request-list.component.html',
    styleUrls: ['./request-list.component.scss']
})
export class RequestListComponent implements OnInit {
    requests: MachineTransferRequest[] = [];
    displayedColumns: string[] = ['machine', 'fromUser', 'toUser', 'type', 'reason', 'created', 'actions'];
    loading = true;

    constructor(
        private requestService: RequestService,
        private snackBar: MatSnackBar
    ) { }

    ngOnInit(): void {
        this.loadRequests();
    }

    loadRequests() {
        this.loading = true;
        this.requestService.getPendingRequests().subscribe({
            next: (data) => {
                this.requests = data;
                this.loading = false;
            },
            error: (err) => {
                this.snackBar.open('Failed to load requests', 'Close', { duration: 3000 });
                this.loading = false;
            }
        });
    }

    approve(id: number) {
        if (confirm('Approve this request?')) {
            this.requestService.approveRequest(id).subscribe({
                next: () => {
                    this.snackBar.open('Request approved', 'Close', { duration: 3000 });
                    this.loadRequests();
                },
                error: (err) => this.snackBar.open('Failed to approve request', 'Close', { duration: 3000 })
            });
        }
    }

    reject(id: number) {
        if (confirm('Reject this request?')) {
            this.requestService.rejectRequest(id).subscribe({
                next: () => {
                    this.snackBar.open('Request rejected', 'Close', { duration: 3000 });
                    this.loadRequests();
                },
                error: (err) => this.snackBar.open('Failed to reject request', 'Close', { duration: 3000 })
            });
        }
    }
}
