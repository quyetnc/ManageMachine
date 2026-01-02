import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
    selector: 'app-user-profile',
    templateUrl: './user-profile.component.html',
    styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
    user: any;

    constructor(private authService: AuthService, private router: Router) { }

    ngOnInit(): void {
        this.user = this.authService.currentUserValue;
    }

    logout() {
        this.authService.logout();
    }

    goBack() {
        this.router.navigate(['/public']);
    }
}
