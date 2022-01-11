import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/shared/services/auth.service';
import { IResetPassword } from 'src/app/shared/models/reset-password';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
})
export class ResetPasswordComponent implements OnInit {
  model: IResetPassword;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _toastrService: ToastrService,
    private _authService: AuthService
  ) {
    this.model = {
      password: '',
      confirmedPassword: '',
      email: '',
      token: '',
    };
  }

  ngOnInit() {
    this._route.queryParams.subscribe((params) => {
      this.model.email = params['email'];
      this.model.token = params['token'];
    });
  }

  passwordValidation() {
    return !!!(this.model.password == '' || this.model.password.length < 1);
  }

  confirmedPasswordValidation() {
    return !!!(
      this.model.confirmedPassword == '' ||
      this.model.confirmedPassword.length < 1
    );
  }

  passwordMatchValidation() {
    return !!!(
      this.model.password != this.model.confirmedPassword &&
      this.confirmedPasswordValidation()
    );
  }

  resetPassword() {
    this._authService.resetPassword(this.model).subscribe((response) => {
      this._toastrService.success('Password successfully restarted.');
      this.goToLogin();
    });
  }

  save() {
    if (
      this.passwordValidation() &&
      this.confirmedPasswordValidation() &&
      this.passwordMatchValidation()
    ) {
      this.resetPassword();
    }
  }

  goToLogin() {
    this._router.navigate(['/login']);
  }
}
