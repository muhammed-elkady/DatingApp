import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from "@auth0/angular-jwt";
import { DecodedToken } from './models/decoded-token';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  private _jwtHelper = new JwtHelperService();

  constructor(private _authService: AuthService) { }

  ngOnInit(): void {
    debugger;
    if (this._authService.isUserLoggedin) {
      const token = localStorage.getItem('token');
      this._authService.decodedToken = this._jwtHelper.decodeToken(token) as DecodedToken;
    }
  }



}
