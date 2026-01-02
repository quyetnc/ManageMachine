import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ParametersRoutingModule } from './parameters-routing.module';
import { ParameterListComponent } from './parameter-list/parameter-list.component';
import { ParameterFormComponent } from './parameter-form/parameter-form.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    ParameterListComponent,
    ParameterFormComponent
  ],
  imports: [
    CommonModule,
    ParametersRoutingModule,
    SharedModule
  ]
})
export class ParametersModule { }
