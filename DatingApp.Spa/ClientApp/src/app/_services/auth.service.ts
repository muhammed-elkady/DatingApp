import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { JwtHelperService } from "@auth0/angular-jwt";
import { DecodedToken } from '../models/interfaces/decoded-token';
import { AlertifyService } from './alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'https://localhost:44378/api/auth/';
  private _jwtHelper = new JwtHelperService();
  public decodedToken: DecodedToken;

  constructor(private httpClient: HttpClient, private alertifyService: AlertifyService) { }
  login(loginModel: any) {
    return this.httpClient.post(this.baseUrl + 'login', loginModel).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this._jwtHelper.decodeToken(user.token) as DecodedToken;
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
  }

  get isUserLoggedin(): boolean {
    const token = localStorage.getItem('token');
    return !this._jwtHelper.isTokenExpired(token)
  }
  get token(): string {
    return localStorage.getItem('token');
  }

}
