import { User } from './../models/interfaces/user';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, BehaviorSubject } from 'rxjs';
import { JwtHelperService } from "@auth0/angular-jwt";
import { DecodedToken } from '../models/interfaces/decoded-token';
import { AlertifyService } from './alertify.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = environment.apiUrl + 'auth/';
  private _jwtHelper = new JwtHelperService();
  public decodedToken: DecodedToken;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/default-user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private httpClient: HttpClient, private alertifyService: AlertifyService) { }

  login(loginModel: any) {
    return this.httpClient.post(this.baseUrl + 'login', loginModel).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.decodedToken = this._jwtHelper.decodeToken(user.token) as DecodedToken;
          this.currentUser = user.user;
          this.changeMemberPhoto(this.currentUser.photoUrl);
          this.alertifyService.success(`Hello ${this.decodedToken.unique_name}`);
        }
      })
    )
  }

  register(user: User): Observable<any> {
    return this.httpClient.post(this.baseUrl + 'register', user);
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.decodedToken = null;
    this.currentUser = null;
  }

  changeMemberPhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }

  get isUserLoggedin(): boolean {
    const token = localStorage.getItem('token');
    return !this._jwtHelper.isTokenExpired(token)
  }
  get token(): string {
    return localStorage.getItem('token');
  }

  roleMatch(allowedRoles): boolean {
    let isMatch = false;
    const userRoles: any = this.decodedToken.role as Array<string>;
    allowedRoles.forEach(element => {
      if (userRoles.includes(element)) {
        isMatch = true;
        return isMatch;
      }
    });
    return isMatch;


  }

}
