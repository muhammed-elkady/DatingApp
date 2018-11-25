import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  // username: string;
  loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  });


  constructor(public authService: AuthService, private _alertifyService: AlertifyService) { }

  ngOnInit() {
    // The reason this didn't work because the value was set and localstorage changes isn't listned to
    // this.username = this._authService.decodedToken.unique_name;
  }


  get isUserLoggedin() {
    return this.authService.isUserLoggedin;
  }

  onLoginFormSubmit() {
    let loginFormToSend = { username: this.loginForm.controls.username.value, password: this.loginForm.controls.password.value }

    this.authService.login(loginFormToSend)
      .subscribe((next) => {
        this._alertifyService.success('logged in successfully');
      },
        (error) => {
          this._alertifyService.error('login failed!');
        });
  }

  logout() {
    this.authService.logout();
    this._alertifyService.message('logged out successfully');
  }


}
