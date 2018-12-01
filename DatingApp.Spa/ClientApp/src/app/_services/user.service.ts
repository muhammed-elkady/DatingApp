import { Injectable } from '@angular/core';
import { environment } from "../../environments/environment";

import { Observable } from 'rxjs';
import { User } from '../models/interfaces/user';
import { HttpClient, HttpHeaders } from '@angular/common/http';


const httpHeaders = {
  headers: new HttpHeaders({
    'Authorization': 'Bearer ' + localStorage.getItem('token')
  })
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'users', httpHeaders)
  }

  getUser(id: string): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id, httpHeaders)
  }

}
