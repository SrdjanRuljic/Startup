import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  CanDeactivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, take } from 'rxjs';
import { Role } from '../shared/enums/role';
import { AuthService } from '../shared/services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  isAuthorized: boolean = false;
  isInRole: boolean = false;

  constructor(
    private _router: Router,
    private _toastrService: ToastrService,
    private _authService: AuthService
  ) {
    this._authService
      .getIsAuthorized()
      .pipe(take(1))
      .subscribe((response) => {
        this.isAuthorized = response;
      });
  }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const roles = route.data?.['roles'] as Role[];

    this.checkIfIsInRole(roles);

    if (this.isAuthorized && this.isInRole) {
      return true;
    }
    this.goToHome();
    this._toastrService.error('You are unauthorized.');
    return false;
  }

  checkIfIsInRole(role: any) {
    this._authService
      .isInRole(role)
      .pipe(take(1))
      .subscribe((response) => {
        this.isInRole = response;
      });
  }

  goToHome() {
    this._router.navigate(['/']);
  }
}
