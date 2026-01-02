import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TypeListComponent } from './type-list/type-list.component';
import { TypeFormComponent } from './type-form/type-form.component';

const routes: Routes = [
  { path: '', component: TypeListComponent },
  { path: 'create', component: TypeFormComponent },
  { path: 'edit/:id', component: TypeFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MachineTypesRoutingModule { }
