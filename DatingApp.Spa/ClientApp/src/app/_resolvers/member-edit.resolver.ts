import { Injectable } from "@angular/core";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { UserService } from './../_services/user.service';
import { AlertifyService } from './../_services/alertify.service';
import { Observable, of } from "rxjs";
import { User } from './../models/interfaces/user';
import { catchError } from "rxjs/operators";
import { AuthService } from './../_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {

    constructor(private userService: UserService,
        private router: Router,
        private alertifyService: AlertifyService,
        private authService: AuthService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        let userId = this.authService.decodedToken.nameid;
        return this.userService.getUser(userId)
            .pipe(
                catchError(error => {
                    this.alertifyService.error('Problem retrieving your data');
                    this.router.navigate(['/members']);
                    return of(null);
                })
            )
    }



}