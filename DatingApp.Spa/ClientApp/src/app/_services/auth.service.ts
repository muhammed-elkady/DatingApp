import { User } from './../models/interfaces/user';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
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
  public currentUser: User;

  constructor(private httpClient: HttpClient, private alertifyService: AlertifyService) { }
  login(loginModel: any) {
    return this.httpClient.post(this.baseUrl + 'login', loginModel).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user))
          this.decodedToken = this._jwtHelper.decodeToken(user.token) as DecodedToken;
          this.currentUser = user.user as User;
          this.alertifyService.success(`Hello ${this.decodedToken.unique_name}`);
        }
      })
    )
  }

  register(registerModel: any): Observable<any> {
    return this.httpClient.post(this.baseUrl + 'register', registerModel);
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.currentUser = null;
  }

  get isUserLoggedin(): boolean {
    const token = localStorage.getItem('token');
    return !this._jwtHelper.isTokenExpired(token)
  }
  get token(): string {
    return localStorage.getItem('token');
  }

}
