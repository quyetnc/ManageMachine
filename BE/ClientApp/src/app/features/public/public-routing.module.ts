import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ScanComponent } from './scan/scan.component';
import { MachineDetailComponent } from './machine-detail/machine-detail.component';
import { UserHomeComponent } from './user-home/user-home.component';
import { UserMachineListComponent } from './user-machine-list/user-machine-list.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserRequestListComponent } from './user-request-list/user-request-list.component';
import { MachineHistoryComponent } from './machine-history/machine-history.component';

const routes: Routes = [
  { path: '', component: UserHomeComponent },
  { path: 'profile', component: UserProfileComponent },
  { path: 'types/:id', component: UserMachineListComponent },
  { path: 'my-machines', component: UserMachineListComponent },
  { path: 'machines/:id', component: MachineDetailComponent }, // Supports both ID and GUID
  { path: 'machines/:id/history', component: MachineHistoryComponent },
  { path: 'requests', component: UserRequestListComponent },
  { path: 'scan', component: ScanComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule { }
