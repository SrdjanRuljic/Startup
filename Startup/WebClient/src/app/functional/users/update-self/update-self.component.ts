import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ConfirmationModalService } from 'src/app/shared/services/confirmation-modal.service';
import { UsersService } from 'src/app/shared/services/users.service';
import { IUser } from 'src/app/shared/models/user';

@Component({
  selector: 'app-update-self',
  templateUrl: './update-self.component.html',
  styleUrls: ['./update-self.component.scss'],
})
export class UpdateSelfComponent implements OnInit {
  model: IUser;
  previousUserName: string | null = null;
  previousFirstName: string | null = null;
  previousLastName: string | null = null;

  constructor(
    private _router: Router,
    private _usersService: UsersService,
    private _authService: AuthService,
    private _toastrService: ToastrService,
    private _confirmationModalService: ConfirmationModalService
  ) {
    this.model = {
      id: '0',
      firstName: '',
      lastName: '',
      userName: '',
      email: '',
    };
  }

  ngOnInit() {
    this.getUser();
  }

  getUser() {
    this._usersService.getLoggedIn().subscribe((response) => {
      this.model = response;
      this.previousUserName = this.model.userName;
      this.previousFirstName = this.model.firstName;
      this.previousLastName = this.model.lastName;
    });
  }

  usernameValidation() {
    return !!!(this.model.userName == '' || this.model.userName.length < 1);
  }

  emailValidation() {
    return !!!(this.model.email == '' || this.model.email.length < 1);
  }

  compareUserNames() {
    return !!!(this.previousUserName === this.model.userName);
  }

  compareFirstNames() {
    return !!!(this.previousFirstName === this.model.firstName);
  }

  compareLastNames() {
    return !!!(this.previousLastName === this.model.lastName);
  }

  goToHome() {
    this._router.navigate(['/']);
  }

  update() {
    this._usersService.updateSelf(this.model).subscribe((response) => {
      this._toastrService.success('User successfully updated.');
      this.refreshDisplaName();
      this.goToHome();
    });
  }

  async save() {
    if (this.usernameValidation() && this.emailValidation()) {
      if (this.compareUserNames()) {
        await this.openConfirmationModal();
      } else {
        this.update();
      }
    }
  }

  async openConfirmationModal() {
    const result = await this._confirmationModalService.confirm(
      'Are you sure you want to update username? You will be logged out!'
    );
    if (result) {
      this.update();
      this.logout();
    }
  }

  logout() {
    this._authService.logout();
  }

  getDisplayName() {
    this._usersService.getDisplayName().subscribe((response) => {});
  }

  refreshDisplaName() {
    if (this.compareFirstNames() || this.compareLastNames()) {
      this.getDisplayName();
    }
  }
}
