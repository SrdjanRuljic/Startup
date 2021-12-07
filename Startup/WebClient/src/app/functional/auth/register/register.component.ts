import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/shared/services/auth.service';
import { IRegister } from '../register';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  model: IRegister;

  constructor(
    private _router: Router,
    private _authService: AuthService,
    private _toastrService: ToastrService
  ) {
    this.model = {
      firstName: '',
      lastName: '',
      username: '',
      email: '',
      password: '',
    };
  }

  ngOnInit() {}

  register() {
    if (
      this.usernameValidation() &&
      this.emailValidation() &&
      this.passwordValidation()
    ) {
      this._authService.register(this.model).subscribe((response) => {
        this.goBack();
        this._toastrService.success('Successfully register, check your email.');
      });
    }
  }

  usernameValidation() {
    return !!!(this.model.username == '' || this.model.username.length < 1);
  }

  passwordValidation() {
    return !!!(this.model.password == '' || this.model.password.length < 1);
  }

  emailValidation() {
    return !!!(this.model.email == '' || this.model.email.length < 1);
  }

  goToLogin() {
    this._router.navigate(['/login']);
  }

  goBack() {
    this._router.navigate(['/']);
  }
}
