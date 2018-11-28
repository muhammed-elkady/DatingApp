import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private authService: AuthService,
        private alertifyService: AlertifyService,
        private router: Router
    ) { }

    canActivate(): Observable<boolean> | Promise<boolean> | boolean {

        if (this.authService.isUserLoggedin) {
            return true;
        }
        this.alertifyService.error('please login first');
        this.router.navigate(['/home']);
        return false;
    }
}
