import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

export interface QrData {
  machineName: string;
  qrCodeData: string;
  serialNumber: string;
}

@Component({
  selector: 'app-qr-bulk-view-dialog',
  template: `
    <h2 mat-dialog-title>Print QR Codes ({{ data.length }} selected)</h2>
    <mat-dialog-content class="bulk-qr-content">
      <div class="qr-grid">
        <div class="qr-item" *ngFor="let item of data">
            <div class="cut-line"></div>
            <!-- Ensure elementType is 'img' for better printing support -->
            <qrcode [qrdata]="item.qrCodeData" [width]="150" [errorCorrectionLevel]="'M'" [elementType]="'img'"></qrcode>
            <div class="code-text">{{ item.serialNumber }}</div>
            <div class="machine-name">{{ item.machineName }}</div>
        </div>
      </div>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="print()">Print</button>
      <button mat-button mat-dialog-close>Close</button>
    </mat-dialog-actions>
  `,
  styles: [`
    .bulk-qr-content {
      padding: 20px;
      min-width: 600px;
    }
    .qr-grid {
      display: flex;
      flex-wrap: wrap;
      gap: 20px;
      justify-content: center;
    }
    .qr-item {
      display: flex;
      flex-direction: column;
      align-items: center;
      border: 1px dashed #ccc;
      padding: 10px;
      width: 180px;
      page-break-inside: avoid;
      break-inside: avoid;
    }
    .info {
        text-align: center;
        margin-top: 5px;
    }
    .machine-name {
        display: block;
        font-weight: bold;
        font-size: 0.9rem;
        margin-bottom: 2px;
    }
    .serial-number {
        display: block;
        font-weight: bold;
        color: #000;
        font-size: 1.1rem;
        margin-bottom: 2px;
        text-transform: uppercase;
    }
    .guid-text {
        font-family: monospace;
        color: #666;
        font-size: 0.7rem;
        word-break: break-all;
    }

    /* Global Print Styles */
    @media print {
      /* Hide everything initially */
      body > * {
        visibility: hidden !important; 
        display: none !important;
      }

      /* But show the overlay container (where dialogs live) */
      body > .cdk-overlay-container {
        visibility: visible !important;
        display: block !important;
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        z-index: 9999;
      }

      /* Drill down to valid content */
      .cdk-overlay-container *, 
      .cdk-overlay-pane, 
      mat-dialog-container, 
      .bulk-qr-content, 
      .bulk-qr-content * {
        visibility: visible !important;
      }
      
      /* Reset positioning context for the dialog */
      .cdk-global-overlay-wrapper, .cdk-overlay-pane {
        position: static !important;
        width: 100% !important;
        height: auto !important;
        max-width: none !important;
        transform: none !important;
        display: block !important;
      }

      mat-dialog-container {
        box-shadow: none !important;
        padding: 0 !important;
        margin: 0 !important;
        border: none !important;
        width: 100% !important;
        overflow: visible !important;
      }

      /* Specific content styling */
      .bulk-qr-content {
        position: relative !important;
        width: 100% !important;
        padding: 0 !important;
        margin: 0 !important;
        overflow: visible !important;
      }

      .qr-grid {
        display: flex !important;
        flex-wrap: wrap !important;
        gap: 10px !important;
        justify-content: flex-start !important;
      }

      .qr-item {
        border: 1px solid #000;
        margin: 5px;
        page-break-inside: avoid;
        break-inside: avoid;
      }

      /* Hide dialog decoration */
      h2[mat-dialog-title], mat-dialog-actions, .cdk-overlay-backdrop {
        display: none !important;
      }
    }
  `],
  encapsulation: ViewEncapsulation.None
})
export class QrBulkViewDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<QrBulkViewDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: QrData[]
  ) { }

  print() {
    window.print();
  }
}
