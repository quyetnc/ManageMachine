import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/core/services/user.service';
import { CreateUserDto, UpdateUserDto, UserRole } from 'src/app/core/models/user.model';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  userId: number | null = null;
  roles = [
    { value: UserRole.Admin, label: 'Admin' },
    { value: UserRole.User, label: 'User' }
  ];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      role: [UserRole.User, Validators.required]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.userId = +id;
      this.form.get('password')?.clearValidators();
      this.form.get('password')?.updateValueAndValidity();
      this.loadUser(this.userId);
    }
  }

  loadUser(id: number): void {
    this.userService.getById(id).subscribe({
      next: (user) => {
        this.form.patchValue({
          username: user.username,
          fullName: user.fullName,
          email: user.email,
          role: user.role
        });
        this.form.get('username')?.disable();
        this.form.get('password')?.disable();
      },
      error: (err) => console.error(err)
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    if (this.isEditMode && this.userId) {
      const dto: UpdateUserDto = {
        fullName: this.form.value.fullName,
        email: this.form.value.email,
        role: Number(this.form.value.role)
      };

      this.userService.update(this.userId, dto).subscribe({
        next: () => {
          alert('User updated successfully');
          this.router.navigate(['/users']);
        },
        error: (err) => alert('Failed to update user: ' + (err.error?.message || err.message || err))
      });
    } else {
      const dto: CreateUserDto = {
        ...this.form.value,
        role: Number(this.form.value.role)
      };

      this.userService.create(dto).subscribe({
        next: () => {
          alert('User created successfully');
          this.router.navigate(['/users']);
        },
        error: (err) => alert('Failed to create user: ' + (err.error?.message || err.message || err))
      });
    }
  }
}
