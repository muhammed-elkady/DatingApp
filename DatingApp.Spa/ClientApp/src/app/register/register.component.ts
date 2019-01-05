import { User } from './../models/interfaces/user';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { matchPassword } from '../shared/validators';
import { BsDatepickerConfig } from 'ngx-bootstrap';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  user: User;
  registerForm: FormGroup;
  datepickerConfig: Partial<BsDatepickerConfig>;

  constructor(private _authService: AuthService,
    private _alertify: AlertifyService,
    private _router: Router,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.initRegisterForm();
    this.datepickerConfig = {
      containerClass: 'theme-red'
    };


  }

  private initRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['', [Validators.required]],
      knownAs: ['', [Validators.required]],
      dateOfBirth: [null, [Validators.required]],
      city: ['', [Validators.required]],
      country: ['', [Validators.required]],
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20)]],
      confirmPassword: ['', [Validators.required]]
    },
      { validators: matchPassword })
  }

  onRegisterFormSubmit() {
    if (this.registerForm.valid) {
      
      this.user = Object.assign({}, this.registerForm.value);
      this._authService.register(this.user)
        .subscribe(
          () => {
            this._alertify.success('registeration successful');
            this._router.navigate(['/']);
          },
          (err) => this._alertify.error(`registeration failed: ${err.toString()}`),
          () => {
            this._authService.login(this.user).subscribe(
              () => this._router.navigate(['/members']),
              (err) => { this._alertify.error(err) }
            );
          }
        );
    }
    else {
      this._alertify.error('Registeration form is invalid');
    }

  }


}


