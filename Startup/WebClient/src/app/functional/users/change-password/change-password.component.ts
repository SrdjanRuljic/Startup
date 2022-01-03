import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UsersService } from 'src/app/shared/services/users.service';
import { IChangePassword } from '../change-password';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
})
export class ChangePasswordComponent implements OnInit {
  model: IChangePassword;

  constructor(
    private _router: Router,
    private _usersService: UsersService,
    private _toastrService: ToastrService
  ) {
    this.model = {
      currentPassword: '',
      newPassword: '',
      confirmedPassword: '',
    };
  }

  ngOnInit() {}

  save() {
    if (
      this.currentPasswordValidation() &&
      this.newPasswordValidation() &&
      this.confirmedPasswordValidation() &&
      this.passwordMatchValidation()
    ) {
      this.changePassword();
    }
  }

  currentPasswordValidation() {
    return !!!(
      this.model.currentPassword == '' || this.model.currentPassword.length < 1
    );
  }

  newPasswordValidation() {
    return !!!(
      this.model.newPassword == '' || this.model.newPassword.length < 1
    );
  }

  confirmedPasswordValidation() {
    return !!!(
      this.model.confirmedPassword == '' ||
      this.model.confirmedPassword.length < 1
    );
  }

  passwordMatchValidation() {
    return !!!(
      this.model.newPassword != this.model.confirmedPassword &&
      this.confirmedPasswordValidation()
    );
  }

  changePassword() {
    this._usersService.changePassword(this.model).subscribe((response) => {
      this._toastrService.success('Password successfully changed.');
      this.goToHome();
    });
  }

  goToHome() {
    this._router.navigate(['/']);
  }
}
