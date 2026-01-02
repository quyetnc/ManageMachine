import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MachineTypesRoutingModule } from './machine-types-routing.module';
import { TypeListComponent } from './type-list/type-list.component';
import { TypeFormComponent } from './type-form/type-form.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    TypeListComponent,
    TypeFormComponent
  ],
  imports: [
    CommonModule,
    MachineTypesRoutingModule,
    SharedModule
  ]
})
export class MachineTypesModule { }
