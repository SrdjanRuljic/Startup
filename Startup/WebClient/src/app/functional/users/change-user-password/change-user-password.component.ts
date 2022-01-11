import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IChangeUserPassword } from 'src/app/shared/models/change-user-password';
import { UsersService } from 'src/app/shared/services/users.service';

@Component({
  selector: 'app-change-user-password',
  templateUrl: './change-user-password.component.html',
  styleUrls: ['./change-user-password.component.scss'],
})
export class ChangeUserPasswordComponent implements OnInit {
  model: IChangeUserPassword;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _usersService: UsersService,
    private _toastrService: ToastrService
  ) {
    this.model = {
      id: '',
      newPassword: '',
      confirmedPassword: '',
    };
  }

  ngOnInit() {
    this._route.params.subscribe((params) => {
      let id = params['id'];
      if (id != '0') {
        this.model.id = id;
      }
    });
  }

  save() {
    if (
      this.newPasswordValidation() &&
      this.confirmedPasswordValidation() &&
      this.passwordMatchValidation()
    ) {
      this.changeUserPassword();
    }
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

  changeUserPassword() {
    this._usersService.changeUserPassword(this.model).subscribe((response) => {
      this._toastrService.success('Password successfully changed.');
      this.goToBack();
    });
  }

  goToBack() {
    this._router.navigate(['/users']);
  }
}
