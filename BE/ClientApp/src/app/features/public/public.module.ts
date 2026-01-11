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

import { TransferDialogComponent } from './machine-detail/transfer-dialog.component';
import { UserRequestListComponent } from './user-request-list/user-request-list.component';
import { MachineHistoryComponent } from './machine-history/machine-history.component';

@NgModule({
  declarations: [
    ScanComponent,
    MachineDetailComponent,
    TransferDialogComponent,
    UserHomeComponent,
    UserMachineListComponent,
    UserProfileComponent,
    UserRequestListComponent,
    MachineHistoryComponent
  ],
  imports: [
    CommonModule,
    PublicRoutingModule,
    SharedModule,
    NgxScannerQrcodeModule
  ]
})
export class PublicModule { }
