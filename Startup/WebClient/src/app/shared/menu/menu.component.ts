import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { PERMISSION } from '../../shared/enums/permissions';
import { ConfirmationModalService } from '../services/confirmation-modal.service';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {
  isAuthorized$: Observable<boolean>;
  PERMISSION: typeof PERMISSION = PERMISSION;
  displayName$: Observable<string>;

  constructor(
    private _router: Router,
    private _authService: AuthService,
    private _confirmationModalService: ConfirmationModalService,
    private _usersService: UsersService
  ) {
    this.isAuthorized$ = this._authService.getIsAuthorized$();
    this.displayName$ = this._usersService.getDisplayName$();
  }

  ngOnInit() {}

  async logout() {
    await this.openConfirmationModal();
  }

  async openConfirmationModal() {
    const result = await this._confirmationModalService.confirm(
      'Are you sure you want to log out?'
    );
    if (result) {
      this._authService.logout();
      this._usersService.setDisplayName$(null);
    }
  }

  goToHome() {
    this._router.navigate(['/']);
  }

  goToLogin() {
    this._router.navigate(['/login']);
  }

  goToRegister() {
    this._router.navigate(['/register']);
  }

  goToUsers() {
    this._router.navigate(['/users']);
  }

  goToUserForm(id: string) {
    this._router.navigate(['/users/form', id]);
  }
}
