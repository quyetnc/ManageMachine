import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { User, UserRole } from 'src/app/core/models/user.model';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  displayedColumns: string[] = ['id', 'username', 'fullName', 'email', 'role', 'actions'];
  dataSource: MatTableDataSource<User>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private userService: UserService, private snackBar: MatSnackBar) {
    this.dataSource = new MatTableDataSource();
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getAll().subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: (err) => {
        console.error(err);
        this.snackBar.open('Failed to load users', 'Close', { duration: 3000 });
      }
    });
  }

  getRoleName(role: UserRole): string {
    return role === UserRole.Admin ? 'Admin' : 'User';
  }

  deleteUser(user: User): void {
    if (confirm(`Are you sure you want to delete user ${user.username}?`)) {
      this.userService.delete(user.id).subscribe({
        next: () => {
          this.snackBar.open('User deleted', 'Close', { duration: 3000 });
          this.loadUsers();
        },
        error: (err) => {
          this.snackBar.open('Failed to delete user: ' + err.error, 'Close', { duration: 3000 });
        }
      });
    }
  }

  resetPassword(user: User): void {
    const newPassword = prompt(`Enter new password for ${user.username}:`);
    if (newPassword) {
      this.userService.resetPassword(user.id, { newPassword }).subscribe({
        next: () => {
          this.snackBar.open('Password reset successfully', 'Close', { duration: 3000 });
        },
        error: (err) => {
          this.snackBar.open('Failed to reset password: ' + err.error, 'Close', { duration: 3000 });
        }
      });
    }
  }
}
