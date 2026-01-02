import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { SafeUrl } from '@angular/platform-browser';

@Component({
    selector: 'app-qr-view-dialog',
    template: `
    <h2 mat-dialog-title>QR Code: {{ data.machineName }}</h2>
    <mat-dialog-content class="qr-content">
      <qrcode [qrdata]="data.qrCodeData" [width]="256" [errorCorrectionLevel]="'M'"></qrcode>
      <div class="guid-text">{{ data.qrCodeData }}</div>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="print()">Print</button>
      <button mat-button mat-dialog-close>Close</button>
    </mat-dialog-actions>
  `,
    styles: [`
    .qr-content {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 20px;
    }
    .guid-text {
        margin-top: 10px;
        font-family: monospace;
        color: #666;
    }
    @media print {
      body * {
        visibility: hidden;
      }
      .qr-content, .qr-content * {
        visibility: visible;
      }
      .qr-content {
        position: absolute;
        left: 0;
        top: 0;
      }
    }
  `]
})
export class QrViewDialogComponent {
    constructor(
        public dialogRef: MatDialogRef<QrViewDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { qrCodeData: string, machineName: string }
    ) { }

    print() {
        window.print();
    }
}
