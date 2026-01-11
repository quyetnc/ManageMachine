import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  loading = false;
  returnUrl: string;
  private bgTimestamp = new Date().getTime();

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) {
    if (this.authService.currentUserValue) {
      this.router.navigate(['/']);
    }
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  get backgroundImage() {
    // Use cached timestamp to avoid NG0100 error
    return `url('${environment.apiUrl.replace('/api', '')}/assets/images/login-bg.png?v=${this.bgTimestamp}')`;
  }

  get f() { return this.loginForm.controls; }

  onSubmit() {
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.authService.login(this.loginForm.value)
      .subscribe({
        next: () => {
          // Role Based Redirect
          const user = this.authService.currentUserValue;
          if (user?.role === 'Admin') {
            this.router.navigate(['/dashboard']); // Explicitly go to dashboard
          } else {
            this.router.navigate(['/public']);
          }
        },
        error: error => {
          this.snackBar.open(error.error?.message || 'Login failed', 'Close', { duration: 3000 });
          this.loading = false;
        }
      });
  }
}
