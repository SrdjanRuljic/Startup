import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../shared/services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  isAuthorized$: Observable<boolean>;
  isInRole: boolean = false;

  constructor(
    private _router: Router,
    private _toastrService: ToastrService,
    private _authService: AuthService
  ) {
    this.isAuthorized$ = this._authService.getIsAuthorized();
  }

  canActivate(route: ActivatedRouteSnapshot) {
    const role = route.data?.['role'] as string;

    if (this.isAuthorized$) {
      return this._authService.isInRole(role).pipe(
        map((isInRole) => {
          if (isInRole) {
            return true;
          } else {
            this.goToHome();
            this._toastrService.error('Unauthorized.');
            return false;
          }
        })
      );
    }

    this.goToHome();
    this._toastrService.error('Unauthorized.');
    return false;
  }

  goToHome() {
    this._router.navigate(['/']);
  }
}
