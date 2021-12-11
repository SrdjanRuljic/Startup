import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../shared/services/auth.service';
import { NgxPermissionsService } from 'ngx-permissions';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private _router: Router,
    private _toastrService: ToastrService,
    private _authService: AuthService,
    private _permissionsService: NgxPermissionsService
  ) {}

  canActivate(route: ActivatedRouteSnapshot) {
    const permission = route.data?.['permission'] as string;
    const isAuthorized = this._authService.getIsAuthorized();

    if (!isAuthorized) {
      this.goToHome();
      this._toastrService.error('Unauthorized.');
      return false;
    }

    return this._permissionsService
      .hasPermission(permission)
      .then((response) => {
        if (response) {
          return true;
        } else {
          this.goToHome();
          this._toastrService.error('Unauthorized.');
          return false;
        }
      });
  }

  goToHome() {
    this._router.navigate(['/']);
  }
}
