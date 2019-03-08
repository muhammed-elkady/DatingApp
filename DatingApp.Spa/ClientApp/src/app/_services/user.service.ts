import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";

import { Observable } from "rxjs";
import { User } from "../models/interfaces/user";
import { HttpClient, HttpParams } from "@angular/common/http";
import { PaginatedResult } from "../models/interfaces/pagination";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: "root"
})
export class UserService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getUsers(
    page?,
    itemsPerPage?,
    userParams?: any
  ): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<
      User[]
    >();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerPage);
    }

    if (userParams != null) {
      params = params.append("minAge", userParams.minAge);
      params = params.append("maxAge", userParams.maxAge);
      params = params.append("gender", userParams.gender);
      params = params.append("orderBy", userParams.orderBy);
    }

    return this.http
      .get<User[]>(this.baseUrl + "users", {
        observe: "response",
        params: params
      })
      .pipe(
        map(res => {
          paginatedResult.result = res.body;
          if (res.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(
              res.headers.get("Pagination")
            );
          }
          return paginatedResult;
        })
      );
  }

  getUser(id: string): Observable<User> {
    return this.http.get<User>(this.baseUrl + "users/" + id);
  }

  updateUser(id: string, user: User) {
    return this.http.put(`${this.baseUrl}users/${id}`, user);
  }

  setMainPhoto(userId: string, photoId: number) {
    return this.http.post(
      `${this.baseUrl}users/${userId}/photos/${photoId}/setmain`,
      {}
    );
  }

  deletePhoto(userId: string, photoId: number) {
    return this.http.delete(
      `${this.baseUrl}/users/${userId}/photos/${photoId}`
    );
  }

  sendLike(id: string, recipientId: string) {
    return this.http.post(
      this.baseUrl + "users/" + id + "/like/" + recipientId,
      {}
    );
  }
}
