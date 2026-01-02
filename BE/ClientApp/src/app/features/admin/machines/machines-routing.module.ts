import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MachineListComponent } from './machine-list/machine-list.component';
import { MachineFormComponent } from './machine-form/machine-form.component';

const routes: Routes = [
  { path: '', component: MachineListComponent },
  { path: 'create', component: MachineFormComponent },
  { path: 'edit/:id', component: MachineFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MachinesRoutingModule { }
