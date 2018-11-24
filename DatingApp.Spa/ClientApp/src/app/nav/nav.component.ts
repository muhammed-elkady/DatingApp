import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../_services/auth.service';

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


  constructor(private authService: AuthService) { }

  ngOnInit() {

  }


  get isUserLoggedin() {
    return this.authService.isUserLoggedin;
  }

  onLoginFormSubmit() {
    let loginFormToSend = { username: this.loginForm.controls.username.value, password: this.loginForm.controls.password.value }
    
    this.authService.login(loginFormToSend)
      .subscribe((next) => {
        console.log(next);
      },
        (error) => {
          console.log(error);
        }
      )
  }

  logout() {
    this.authService.logout();
  }


}
