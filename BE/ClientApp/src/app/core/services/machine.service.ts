import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { environment } from 'src/environments/environment';

export interface Machine {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
  serialNumber: string;
  machineTypeId: number;
  machineTypeName: string;
  machineType?: MachineType;
  parameters: MachineParameter[];
  userId?: number;
  userFullName?: string;
  tenantId?: number;
  tenantName?: string;
  status?: string;
  pendingTransferRequestId?: number;
}

export interface MachineParameter {
  id?: number;
  parameterId: number;
  parameterName: string;
  parameterUnit: string;
  value: string;
}

export interface MachineType {
  id: number;
  name: string;
  description: string;
}

export interface Parameter {
  id: number;
  name: string;
  unit: string;
  description: string;
}

export interface CreateMachineParameterDto {
  parameterId: number;
  value: string;
}

@Injectable({
  providedIn: 'root'
})
export class MachineService {

  constructor(private api: ApiService, private http: HttpClient) { }

  // Machines
  getMachines(): Observable<Machine[]> {
    return this.api.get('Machines');
  }

  getMachine(id: number): Observable<Machine> {
    return this.api.get(`Machines/${id}`);
  }

  getMachineByQr(code: string): Observable<Machine> {
    return this.http.get<Machine>(`${environment.apiUrl}/Machines/qr/${code}`);
  }

  getMachineByCode(code: string): Observable<Machine> {
    return this.http.get<Machine>(`${environment.apiUrl}/Machines/by-code/${code}`);
  }

  getMyMachines(): Observable<Machine[]> {
    return this.api.get<Machine[]>(`Machines/mine`);
  }

  createMachine(machine: FormData): Observable<Machine> {
    return this.api.post('Machines', machine);
  }

  updateMachine(id: number, data: any): Observable<void> {
    return this.api.put(`Machines/${id}`, data);
  }

  deleteMachine(id: number): Observable<void> {
    return this.api.delete(`Machines/${id}`);
  }

  // Machine Types
  getMachineTypes(): Observable<MachineType[]> {
    return this.api.get('types'); // Match backend route "api/types"
  }

  getMachineType(id: number): Observable<MachineType> {
    return this.api.get(`types/${id}`);
  }

  createMachineType(data: any): Observable<MachineType> {
    return this.api.post('types', data);
  }

  updateMachineType(id: number, data: any): Observable<void> {
    return this.api.put(`types/${id}`, data);
  }

  deleteMachineType(id: number): Observable<void> {
    return this.api.delete(`types/${id}`);
  }

  // Parameters
  getParameters(): Observable<Parameter[]> {
    return this.api.get('parameters'); // Match backend route "api/parameters"
  }

  getParameter(id: number): Observable<Parameter> {
    return this.api.get(`parameters/${id}`);
  }

  createParameter(data: any): Observable<Parameter> {
    return this.api.post('parameters', data);
  }

  // New method: addParameter
  addParameter(machineId: number, param: CreateMachineParameterDto): Observable<void> {
    // Assuming api.post can handle the DTO and constructs the URL correctly
    return this.api.post<void>(`Machines/${machineId}/parameters`, param);
  }

  // New method: uploadImage
  uploadImage(file: File): Observable<{ url: string }> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ url: string }>(`${environment.apiUrl}/upload`, formData);
  }

  returnMachine(id: number): Observable<any> {
    return this.http.post(`${environment.apiUrl}/Machines/${id}/return`, {});
  }

  updateParameter(id: number, data: any): Observable<void> {
    return this.api.put(`parameters/${id}`, data);
  }

  deleteParameter(id: number): Observable<void> {
    return this.api.delete(`parameters/${id}`);
  }

  getHistory(id: number): Observable<any[]> {
    return this.api.get<any[]>(`Machines/${id}/history`);
  }
}
