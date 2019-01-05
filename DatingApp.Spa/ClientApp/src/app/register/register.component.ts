import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { matchPassword } from '../shared/validators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registerModel: any = {};
  registerForm: FormGroup;

  constructor(private _authService: AuthService,
    private _alertifyService: AlertifyService,
    private _router: Router,
    private fb: FormBuilder) { }]

  ngOnInit() {
    this.initRegisterForm();

  }

  private initRegisterForm() {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(20)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: matchPassword })
  }

  onRegisterFormSubmit() {
    if (this.registerForm.valid) {
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


