import { User } from './models/interfaces/user';
import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from "@auth0/angular-jwt";
import { DecodedToken } from './models/interfaces/decoded-token';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  private _jwtHelper = new JwtHelperService();

  constructor(private _authService: AuthService) { }

  ngOnInit(): void {
    if (this._authService.isUserLoggedin) {
      const token = localStorage.getItem('token');
      this._authService.decodedToken = this._jwtHelper.decodeToken(token) as DecodedToken;
    }
    const user: User = JSON.parse(localStorage.getItem('user'));
    if (user) {
      this._authService.currentUser = user;
    }
  }

}
