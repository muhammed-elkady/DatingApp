import { Injectable } from '@angular/core';
import { environment } from "../../environments/environment";

import { Observable } from 'rxjs';
import { User } from '../models/interfaces/user';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

export class UserService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'users')
  }

  getUser(id: string): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id)
  }

  updateUser(id: string, user: User) {
    return this.http.put(`${this.baseUrl}users/${id}`, user)
  }

  setMainPhoto(userId: string, photoId: number) {
    return this.http.post(`${this.baseUrl}users/${userId}/photos/${photoId}/setmain`, {});
  }

  deletePhoto(userId: string, photoId: number) {
    return this.http.delete(`${this.baseUrl}/users/${userId}/photos/${photoId}`);
  }

}
