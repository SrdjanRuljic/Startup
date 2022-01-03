import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/shared/services/auth.service';
import { IForgotPassword } from '../forgot-password';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
})
export class ForgotPasswordComponent implements OnInit {
  model: IForgotPassword;

  constructor(
    private _router: Router,
    private _authService: AuthService,
    private _toastrService: ToastrService
  ) {
    this.model = {
      email: '',
      clientUri: 'http://localhost:4200/reset-password',
    };
  }

  ngOnInit() {}

  save() {
    if (this.emailValidation()) {
      this.forgotPassword();
    }
  }

  emailValidation() {
    return !!!(this.model.email == '' || this.model.email.length < 1);
  }

  forgotPassword() {
    this._authService.forgotPassword(this.model).subscribe((response) => {
      this._toastrService.success(
        'Request successfully sent, check your email.'
      );
      this.goToHome();
    });
  }

  goToHome() {
    this._router.navigate(['/']);
  }
}
