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
  roles: Array<any> = [
    { name: PERMISSION.BASIC, value: PERMISSION.BASIC },
    { name: PERMISSION.MODERATOR, value: PERMISSION.MODERATOR },
  ];
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
      roles: [],
    };
  }

  ngOnInit() {
    this._route.params.subscribe((params) => {
      let id = params['id'];
      if (id != '0') {
        this.getUser(id);
      }
    });
  }

  getUser(id: string) {}

  onCheckboxChange(e: any) {
    if (e.target.checked) {
      this.model.roles.push(e.target.value);
    } else {
      let i: number = 0;
      this.model.roles.forEach((role: string) => {
        if (role == e.target.value) {
          this.model.roles.splice(i, 1);
          return;
        }
        i++;
      });
    }
  }

  usernameValidation() {
    return !!!(this.model.username == '' || this.model.username.length < 1);
  }

  emailValidation() {
    return !!!(this.model.email == '' || this.model.email.length < 1);
  }

  rolesValidation() {
    return !!!(this.model.roles.length < 1);
  }

  goBack() {
    this._router.navigate(['/users']);
  }

  save() {
    if (
      this.usernameValidation() &&
      this.emailValidation() &&
      this.rolesValidation()
    ) {
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
