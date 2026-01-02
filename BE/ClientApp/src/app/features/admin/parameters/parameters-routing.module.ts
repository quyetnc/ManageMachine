import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ParameterListComponent } from './parameter-list/parameter-list.component';
import { ParameterFormComponent } from './parameter-form/parameter-form.component';

const routes: Routes = [
  { path: '', component: ParameterListComponent },
  { path: 'create', component: ParameterFormComponent },
  { path: 'edit/:id', component: ParameterFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ParametersRoutingModule { }
