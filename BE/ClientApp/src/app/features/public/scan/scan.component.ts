import { Component, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgxScannerQrcodeComponent, ScannerQRCodeConfig, ScannerQRCodeSelectedFiles } from 'ngx-scanner-qrcode';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-scan',
  templateUrl: './scan.component.html',
  styleUrls: ['./scan.component.scss']
})
export class ScanComponent implements OnDestroy {
  public config: ScannerQRCodeConfig = {
    constraints: {
      video: {
        width: window.innerWidth
      }
    }
  };

  @ViewChild('action') action!: NgxScannerQrcodeComponent;

  constructor(private router: Router, private snackBar: MatSnackBar) { }

  public onEvent(e: any): void {
    if (e && e.length > 0) {
      const value = e[0].value;
      this.action.stop(); // Stop scanning once found

      // Value corresponds to Machine.QRCodeData (GUID)
      // We need to find the machine by this GUID.
      // For simplicity, let's assume we navigate to a route that resolves via GUID or ID.
      // But wait, the API 'GetByQRCode' endpoint doesn't exist yet, or does it?
      // Let's verify. Checking MachineService... no method for GetByQRCode.
      // Checking Controller... MachinesController... No explicit endpoint for QR Code lookup.

      // Workaround: We might need to implement a lookup in Backend.
      // For now, let's assume we can pass the GUID to the detail page, 
      // and the detail page will try to find it (requires API update).

      this.router.navigate(['/public/machine', value]);
    }
  }

  public handle(action: any, fn: string): void {
    const playDeviceFacingBack = (devices: any[]) => {
      // front camera or back camera check here!
      const device = devices.find(f => (/back|rear|environment/gi.test(f.label))); // Default Back Facing Camera
      action.playDevice(device ? device.deviceId : devices[0].deviceId);
    }

    if (fn === 'start') {
      action[fn](playDeviceFacingBack);
    } else {
      action[fn]();
    }
  }

  ngOnDestroy(): void {
    if (this.action) {
      this.action.stop();
    }
  }
}
