import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { PERMISSION } from 'src/app/shared/enums/permissions';
import { UsersService } from 'src/app/shared/services/users.service';
import { IUser } from '../user';

@Component({
  selector: 'app-users-form',
  templateUrl: './users-form.component.html',
  styleUrls: ['./users-form.component.scss'],
})
export class UsersFormComponent implements OnInit {
  model: IUser;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _usersService: UsersService,
    private _toastrService: ToastrService
  ) {
    this.model = {
      id: '0',
      firstName: '',
      lastName: '',
      username: '',
      email: '',
      roles: [PERMISSION.BASIC],
    };
  }

  ngOnInit() {
    this._route.params.subscribe((params) => {
      let id = params['id'];
      if (!isNaN(id) && id != '0') {
        this.getUser(id);
      }
    });
  }

  getUser(id: string) {}

  usernameValidation() {
    return !!!(this.model.username == '' || this.model.username.length < 1);
  }

  emailValidation() {
    return !!!(this.model.email == '' || this.model.email.length < 1);
  }

  goBack() {
    this._router.navigate(['/users']);
  }

  save() {
    if (this.usernameValidation() && this.emailValidation()) {
      if (this.model.id == '0') {
        this.insert();
      } else {
        this.update();
      }
    }
  }

  insert() {
    this._usersService.insert(this.model).subscribe((response) => {
      this._toastrService.success('User successfully added.');
      this.goBack();
    });
  }

  update() {
    this._usersService.update(this.model).subscribe((response) => {
      this._toastrService.success('User successfully updated.');
      this.goBack();
    });
  }
}
