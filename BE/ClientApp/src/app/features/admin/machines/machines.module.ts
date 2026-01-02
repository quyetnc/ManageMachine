import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MachinesRoutingModule } from './machines-routing.module';
import { MachineListComponent } from './machine-list/machine-list.component';
import { MachineFormComponent } from './machine-form/machine-form.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    MachineListComponent,
    MachineFormComponent
  ],
  imports: [
    CommonModule,
    MachinesRoutingModule,
    SharedModule
  ]
})
export class MachinesModule { }
