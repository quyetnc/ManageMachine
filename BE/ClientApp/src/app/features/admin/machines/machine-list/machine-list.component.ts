import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { Machine, MachineService, MachineType } from 'src/app/core/services/machine.service';
import { UserService, User } from 'src/app/core/services/user.service';
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

  // Filter Data
  users: User[] = [];
  machineTypes: MachineType[] = [];

  // Filter Values
  filterValues = {
    name: '',
    type: '',
    owner: ''
  };

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private machineService: MachineService,
    private userService: UserService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog,
    private router: Router
  ) {
    this.dataSource = new MatTableDataSource();
    this.dataSource.filterPredicate = this.createFilter();
  }

  ngOnInit(): void {
    this.loadMachines();
    this.loadAuxiliaryData();
  }

  loadAuxiliaryData() {
    this.userService.getAll().subscribe(users => this.users = users);
    this.machineService.getMachineTypes().subscribe(types => this.machineTypes = types);
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

  applyFilter(filterValue: string, key: string) {
    this.filterValues[key as keyof typeof this.filterValues] = filterValue.trim().toLowerCase();
    this.dataSource.filter = JSON.stringify(this.filterValues);

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  createFilter(): (data: Machine, filter: string) => boolean {
    return (data: Machine, filter: string): boolean => {
      const searchTerms = JSON.parse(filter);

      // Name or Code filter (global search input)
      const name = data.name ? data.name.toLowerCase() : '';
      const serial = data.serialNumber ? data.serialNumber.toLowerCase() : '';
      const filterName = searchTerms.name.toLowerCase();

      const nameMatch = name.includes(filterName) || serial.includes(filterName);

      // Type Filter
      const typeMatch = searchTerms.type ? data.machineTypeName?.toLowerCase() === searchTerms.type : true;

      // Owner Filter
      // machine.userId -> Owner
      const ownerMatch = searchTerms.owner ? data.userId?.toString() === searchTerms.owner : true;

      return nameMatch && typeMatch && ownerMatch;
    };
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
      qrCodeData: m.serialNumber,
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
