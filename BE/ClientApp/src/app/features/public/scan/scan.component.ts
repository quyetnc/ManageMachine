import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgxScannerQrcodeComponent, ScannerQRCodeConfig, ScannerQRCodeResult } from 'ngx-scanner-qrcode';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-scan',
  templateUrl: './scan.component.html',
  styleUrls: ['./scan.component.scss']
})
export class ScanComponent implements AfterViewInit, OnDestroy {
  public config: ScannerQRCodeConfig = {
    constraints: {
      video: {
        facingMode: 'environment' // Use rear camera by default
      }
    }
  };

  @ViewChild('action') action!: NgxScannerQrcodeComponent;

  public isLoading = true;
  public hasPermission = true;

  constructor(private router: Router, private snackBar: MatSnackBar) { }

  ngAfterViewInit(): void {
    // Auto start scanner
    setTimeout(() => {
      this.action.start();
      this.isLoading = false;

      // Subscribe to data emissions
      this.action.data.subscribe((data: ScannerQRCodeResult[]) => {
        if (data && data.length > 0) {
          console.log('Scanner data emitted:', data);
          const value = data[0].value;
          if (value) {
            this.handleScanSuccess(value);
          }
        }
      });
    }, 500);
  }

  public onEvent(e: ScannerQRCodeResult[]): void {
    // Keep this as backup or for template binding
    if (e && e.length > 0) {
      console.log('Scanner event binding:', e);
      const value = e[0].value;
      if (value) {
        this.handleScanSuccess(value);
      }
    }
  }

  private handleScanSuccess(value: string) {
    console.log('Scan success. Value:', value);
    // Debounce or check if already processing to avoid multiple navigations
    if (this.action.isStart) {
      this.action.stop();
      this.action.isPause = true; // Force pause
      this.router.navigate(['/public/machines', value]);
    }
  }

  public handle(action: any, fn: string): void {
    const playDeviceFacingBack = (devices: any[]) => {
      // front camera or back camera check here!
      const device = devices.find(f => (/back|rear|environment/gi.test(f.label))); // Default Back Facing Camera
      action.playDevice(device ? device.deviceId : devices[0].deviceId);
    }

    if (fn === 'togglePlay') {
      if (action.isStart) {
        action.stop();
      } else {
        action.start(playDeviceFacingBack).subscribe({
          error: (err: any) => {
            console.error('Scanner error', err);
            this.snackBar.open('Could not start camera. Please check permissions.', 'Close', { duration: 3000 });
            this.hasPermission = false;
          }
        });
      }
    } else if (fn === 'toggleTorcher') {
      if (action.isStart) {
        action.toggleTorcher();
      }
    }
  }

  ngOnDestroy(): void {
    if (this.action && this.action.isStart) {
      this.action.stop();
    }
  }

  goBack() {
    this.router.navigate(['/public']);
  }
}
