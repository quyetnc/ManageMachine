import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ScanComponent } from './scan/scan.component';
import { MachineDetailComponent } from './machine-detail/machine-detail.component';

const routes: Routes = [
  { path: 'scan', component: ScanComponent },
  { path: 'machine/:id', component: MachineDetailComponent }, // :id here is the GUID from QR
  { path: '', redirectTo: 'scan', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule { }
