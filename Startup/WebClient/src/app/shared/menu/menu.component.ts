import { Component, OnDestroy, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { BsDropdownConfig } from 'ngx-bootstrap/dropdown';
import { PERMISSION } from '../../shared/enums/permissions';
import { ConfirmationModalService } from '../services/confirmation-modal.service';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
  providers: [
    {
      provide: BsDropdownConfig,
      useValue: { isAnimated: true, autoClose: true },
    },
  ],
})
export class MenuComponent implements OnInit, OnDestroy {
  isAuthorized$: Observable<boolean>;
  PERMISSION: typeof PERMISSION = PERMISSION;
  displayName: string = '';
  subscription: Subscription;

  constructor(
    private _router: Router,
    private _authService: AuthService,
    private _confirmationModalService: ConfirmationModalService,
    private _usersService: UsersService
  ) {
    this.isAuthorized$ = this._authService.getIsAuthorized$();
    this.subscription = this._usersService.displayName$.subscribe(
      (response) => {
        this.displayName = response;
      }
    );
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
      this._authService.logout().subscribe((response) => {
        this._usersService.displayName = '';
        this.goToHome();
      });
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

  goToUpdateSelf() {
    this._router.navigate(['/users/update-self']);
  }

  goToChangePassword() {
    this._router.navigate(['/users/change-password']);
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
