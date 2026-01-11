import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export enum RequestStatus {
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

export enum RequestType {
    Borrow = 0,
    Repair = 1
}

export interface MachineTransferRequest {
    id: number;
    machineId: number;
    machineName: string;
    fromUserId: number;
    fromUserName: string;
    toUserId: number;
    toUserName: string;
    requestType: RequestType;
    status: RequestStatus;
    reason: string;
    createdAt: Date;
}

export interface CreateMachineTransferRequestDto {
    machineId: number;
    toUserId: number;
    requestType: RequestType;
    reason: string;
}

@Injectable({
    providedIn: 'root'
})
export class RequestService {
    private apiUrl = `${environment.apiUrl}/requests`;

    constructor(private http: HttpClient) { }

    createRequest(dto: CreateMachineTransferRequestDto): Observable<any> {
        return this.http.post(this.apiUrl, dto);
    }

    getPendingRequests(): Observable<MachineTransferRequest[]> {
        return this.http.get<MachineTransferRequest[]>(this.apiUrl);
    }

    approveRequest(id: number): Observable<any> {
        return this.http.post(`${this.apiUrl}/${id}/approve`, {});
    }

    rejectRequest(id: number): Observable<any> {
        return this.http.post(`${this.apiUrl}/${id}/reject`, {});
    }

    cancelRequest(id: number): Observable<any> {
        return this.http.post(`${this.apiUrl}/${id}/cancel`, {});
    }

    getMyRequests(): Observable<MachineTransferRequest[]> {
        return this.http.get<MachineTransferRequest[]>(`${this.apiUrl}/my-requests`);
    }
}
