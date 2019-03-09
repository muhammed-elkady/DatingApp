import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(private authService: AuthService,
        private alertifyService: AlertifyService,
        private router: Router
    ) { }

    canActivate(next: ActivatedRouteSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        const roles = next.firstChild.data['roles'] as Array<string>;
        if (roles) {
            const match = this.authService.roleMatch(roles);
            if (match) {
                return true;
            }
            else {
                this.router.navigate(['/members']);
                this.alertifyService.error('Unauthorized area');
            }
        }
        if (this.authService.isUserLoggedin) {
            return true;
        }
        this.alertifyService.error('please login first');
        this.router.navigate(['/home']);
        return false;
    }
}
