import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';

export interface Machine {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
  qrCodeData: string;
  machineTypeId: number;
  machineTypeName: string;
  machineType?: MachineType;
  parameters: MachineParameter[];
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

@Injectable({
  providedIn: 'root'
})
export class MachineService {

  constructor(private api: ApiService) { }

  // Machines
  getMachines(): Observable<Machine[]> {
    return this.api.get('Machines');
  }

  getMachine(id: number): Observable<Machine> {
    return this.api.get(`Machines/${id}`);
  }

  createMachine(data: any): Observable<Machine> {
    return this.api.post('Machines', data);
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

  updateParameter(id: number, data: any): Observable<void> {
    return this.api.put(`parameters/${id}`, data);
  }

  deleteParameter(id: number): Observable<void> {
    return this.api.delete(`parameters/${id}`);
  }
}
