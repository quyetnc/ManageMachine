import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxScannerQrcodeModule } from 'ngx-scanner-qrcode';

import { PublicRoutingModule } from './public-routing.module';
import { ScanComponent } from './scan/scan.component';
import { MachineDetailComponent } from './machine-detail/machine-detail.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { UserHomeComponent } from './user-home/user-home.component';
import { UserMachineListComponent } from './user-machine-list/user-machine-list.component';
import { UserProfileComponent } from './user-profile/user-profile.component';

@NgModule({
  declarations: [
    ScanComponent,
    MachineDetailComponent,
    UserHomeComponent,
    UserHomeComponent,
    UserMachineListComponent,
    UserProfileComponent
  ],
  imports: [
    CommonModule,
    PublicRoutingModule,
    SharedModule,
    NgxScannerQrcodeModule
  ]
})
export class PublicModule { }
