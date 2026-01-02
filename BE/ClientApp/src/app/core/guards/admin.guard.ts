import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
    providedIn: 'root'
})
export class AdminGuard implements CanActivate {
    constructor(
        private router: Router,
        private authService: AuthService
    ) { }

    canActivate(): boolean | UrlTree {
        const user = this.authService.currentUserValue;
        if (user && user.role === 'Admin') {
            return true;
        }

        // Redirect to user portal if logged in but not admin
        if (user) {
            return this.router.createUrlTree(['/public']);
        }

        // Otherwise back to login
        return this.router.createUrlTree(['/auth/login']);
    }
}
