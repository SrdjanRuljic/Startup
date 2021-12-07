import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ILogin } from '../login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  model: ILogin;

  constructor(
    private _router: Router,
    private _authService: AuthService,
    private _toastrService: ToastrService
  ) {
    this.model = {
      username: '',
      password: '',
    };
  }

  ngOnInit() {
    if (this._authService.getToken()) {
      this.goBack();
      this._toastrService.success('Already logged in.');
    }
  }

  login() {
    if (this.usernameValidation() && this.passwordValidation()) {
      this._authService.login(this.model).subscribe((response) => {
        this.goBack();
        this._toastrService.success('Successfully logged in.');
      });
    }
  }

  usernameValidation() {
    return !!!(this.model.username == '' || this.model.username.length < 1);
  }

  passwordValidation() {
    return !!!(this.model.password == '' || this.model.password.length < 1);
  }

  goBack() {
    this._router.navigate(['/']);
  }
}
