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

  registerModel: any = {};
  registerForm: FormGroup;
  datepickerConfig: Partial<BsDatepickerConfig>;

  constructor(private _authService: AuthService,
    private _alertifyService: AlertifyService,
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
    debugger;
    if (this.registerForm.valid) {
      // TODO: Initialize registerModel DTO

      this._authService.register(this.registerModel)
        .subscribe(
          (response) => {
            this._alertifyService.success('registeration successful');
            this._router.navigate(['/']);
          },
          () => this._alertifyService.error('registeration failed')
        );
    }
    else {
      this._alertifyService.error('Registeration form is invalid');
    }

  }


}


