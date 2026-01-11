import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { Machine, MachineService } from 'src/app/core/services/machine.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SelectionModel } from '@angular/cdk/collections';
import { QrBulkViewDialogComponent } from 'src/app/shared/components/qr-bulk-view-dialog.component';

import { Router } from '@angular/router';

@Component({
  selector: 'app-machine-list',
  templateUrl: './machine-list.component.html',
  styleUrls: ['./machine-list.component.scss']
})
export class MachineListComponent implements OnInit {
  displayedColumns: string[] = ['select', 'image', 'name', 'code', 'type', 'ownership', 'parameters', 'description', 'actions'];
  dataSource: MatTableDataSource<Machine>;
  selection = new SelectionModel<Machine>(true, []);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private machineService: MachineService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private router: Router
  ) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit(): void {
    this.loadMachines();
  }

  loadMachines() {
    this.machineService.getMachines().subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: (err) => {
        console.error('Error loading machines', err);
        this.snackBar.open('Failed to load machine list.', 'Close', { duration: 3000 });
      }
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.dataSource.data.forEach(row => this.selection.select(row));
  }

  printSelectedQr() {
    if (this.selection.selected.length === 0) return;

    const qrData = this.selection.selected.map(m => ({
      machineName: m.name,
      qrCodeData: m.qrCodeData,
      serialNumber: m.serialNumber
    }));

    this.dialog.open(QrBulkViewDialogComponent, {
      width: '800px',
      data: qrData
    });
  }

  deleteMachine(id: number) {
    if (confirm('Are you sure you want to delete this machine?')) {
      this.machineService.deleteMachine(id).subscribe({
        next: () => {
          this.snackBar.open('Machine deleted successfully', 'Close', { duration: 3000 });
          this.loadMachines();
          this.selection.clear();
        },
        error: (err) => {
          console.error(err);
          this.snackBar.open('Failed to delete machine', 'Close', { duration: 3000 });
        }
      });
    }
  }



  viewHistory(machine: Machine) {
    this.router.navigate(['/public/machines', machine.id, 'history'], { queryParams: { returnUrl: '/admin/machines' } });
  }
}
