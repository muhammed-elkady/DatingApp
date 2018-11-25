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

  loginForm = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  });


  constructor(private _authService: AuthService, private _alertifyService: AlertifyService) { }

  ngOnInit() {

  }


  get isUserLoggedin() {
    return this._authService.isUserLoggedin;
  }

  onLoginFormSubmit() {
    let loginFormToSend = { username: this.loginForm.controls.username.value, password: this.loginForm.controls.password.value }

    this._authService.login(loginFormToSend)
      .subscribe((next) => {
        console.log(next);
        this._alertifyService.success('logged in successfully');
      },
        (error) => {
          this._alertifyService.error('login failed!');
        }
      )
  }

  logout() {
    this._authService.logout();
    this._alertifyService.message('logged out successfully');
  }


}
