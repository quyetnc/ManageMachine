import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MachineType, MachineService } from 'src/app/core/services/machine.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-type-list',
  templateUrl: './type-list.component.html',
  styleUrls: ['./type-list.component.scss']
})
export class TypeListComponent implements OnInit {
  displayedColumns: string[] = ['name', 'description', 'actions'];
  dataSource: MatTableDataSource<MachineType>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private machineService: MachineService, private snackBar: MatSnackBar) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit(): void {
    this.loadTypes();
  }

  loadTypes() {
    this.machineService.getMachineTypes().subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: (err) => {
        console.error(err);
        this.snackBar.open('Failed to load types', 'Close', { duration: 3000 });
      }
    });
  }

  deleteType(id: number) {
    if (confirm('Are you sure?')) {
      this.machineService.deleteMachineType(id).subscribe({
        next: () => {
          this.snackBar.open('Type deleted', 'Close', { duration: 3000 });
          this.loadTypes();
        },
        error: (err) => {
          console.error(err);
          this.snackBar.open('Failed to delete type. It might be in use.', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
