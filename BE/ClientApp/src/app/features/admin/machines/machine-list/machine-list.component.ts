import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Machine, MachineService } from 'src/app/core/services/machine.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-machine-list',
  templateUrl: './machine-list.component.html',
  styleUrls: ['./machine-list.component.scss']
})
export class MachineListComponent implements OnInit {
  displayedColumns: string[] = ['name', 'type', 'parameters', 'description', 'actions'];
  dataSource: MatTableDataSource<Machine>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private machineService: MachineService, private snackBar: MatSnackBar) {
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

  deleteMachine(id: number) {
    if (confirm('Are you sure you want to delete this machine?')) {
      this.machineService.deleteMachine(id).subscribe({
        next: () => {
          this.snackBar.open('Machine deleted successfully', 'Close', { duration: 3000 });
          this.loadMachines();
        },
        error: (err) => {
          console.error(err);
          this.snackBar.open('Failed to delete machine', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
