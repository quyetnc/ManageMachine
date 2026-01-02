import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxScannerQrcodeModule } from 'ngx-scanner-qrcode';

import { PublicRoutingModule } from './public-routing.module';
import { ScanComponent } from './scan/scan.component';
import { MachineDetailComponent } from './machine-detail/machine-detail.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [
    ScanComponent,
    MachineDetailComponent
  ],
  imports: [
    CommonModule,
    PublicRoutingModule,
    SharedModule,
    NgxScannerQrcodeModule
  ]
})
export class PublicModule { }
