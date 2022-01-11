import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/shared/services/auth.service';
import { IConfirmEmail } from 'src/app/shared/models/confirm-email';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss'],
})
export class ConfirmEmailComponent implements OnInit {
  model: IConfirmEmail;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _toastrService: ToastrService,
    private _authService: AuthService
  ) {
    this.model = {
      userName: '',
      token: '',
    };
  }

  ngOnInit() {
    this._route.queryParams.subscribe((params) => {
      this.model.userName = params['username'];
      this.model.token = params['token'];

      if (this.userNameValidation() && this.tokenValidation()) {
        this.confirmEmail();
      }
    });
  }

  confirmEmail() {
    this._authService.confirmEmail(this.model).subscribe((response) => {
      this._toastrService.success(
        'Account successfully confirmed, try to login.'
      );
      this.goToLogin();
    });
  }

  userNameValidation() {
    return !!!(this.model.userName == '' || this.model.userName.length < 1);
  }

  tokenValidation() {
    return !!!(this.model.token == '' || this.model.token.length < 1);
  }

  goToLogin() {
    this._router.navigate(['/login']);
  }
}
