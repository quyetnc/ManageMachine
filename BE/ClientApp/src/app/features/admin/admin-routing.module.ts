import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminLayoutComponent } from './layout/admin-layout/admin-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      {
        path: 'machines',
        loadChildren: () => import('./machines/machines.module').then(m => m.MachinesModule)
      },
      {
        path: 'machine-types',
        loadChildren: () => import('./machine-types/machine-types.module').then(m => m.MachineTypesModule)
      },
      {
        path: 'parameters',
        loadChildren: () => import('./parameters/parameters.module').then(m => m.ParametersModule)
      },
      {
        path: 'users',
        loadChildren: () => import('./users/users.module').then(m => m.UsersModule)
      },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
