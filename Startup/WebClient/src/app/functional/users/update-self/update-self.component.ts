import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationModalService } from 'src/app/shared/services/confirmation-modal.service';
import { UsersService } from 'src/app/shared/services/users.service';
import { IUser } from '../user';

@Component({
  selector: 'app-update-self',
  templateUrl: './update-self.component.html',
  styleUrls: ['./update-self.component.scss'],
})
export class UpdateSelfComponent implements OnInit {
  model: IUser;
  previousUserName: string | null = null;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _usersService: UsersService,
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
    this._usersService.getByUserName().subscribe((response) => {
      this.model = response;
      this.previousUserName = this.model.userName;
    });
  }

  usernameValidation() {
    return !!!(this.model.userName == '' || this.model.userName.length < 1);
  }

  emailValidation() {
    return !!!(this.model.email == '' || this.model.email.length < 1);
  }

  compareUsernames() {
    return !!!(this.previousUserName === this.model.userName);
  }

  goToHome() {
    this._router.navigate(['/']);
  }

  update() {
    this._usersService.updateSelf(this.model).subscribe((response) => {
      this._toastrService.success('User successfully updated.');
      this.goToHome();
    });
  }

  async save() {
    if (this.usernameValidation() && this.emailValidation()) {
      if (this.compareUsernames()) {
        await this.openConfirmationModal();
      } else {
        this.update();
      }
    }
  }

  async openConfirmationModal() {
    const result = await this._confirmationModalService.confirm(
      'Are you sure you want to update username?'
    );
    if (result) {
      this.update();
    }
  }
}
