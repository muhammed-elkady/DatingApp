import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'https://localhost:44378/api/auth/';
  constructor(private httpClient: HttpClient) { }

  login(loginModel: any): Observable<any> {
    return this.httpClient.post(this.baseUrl + 'login', loginModel).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
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

  get isUserLoggedin() {
    return !!localStorage.getItem('token');
  }

}