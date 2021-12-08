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
import { AuthService } from '../shared/services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  isAuthorized: boolean = false;

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

  canActivate(): boolean {
    if (this.isAuthorized) {
      return true;
    }
    this.goToHome();
    this._toastrService.error('You are unauthorized.');
    return false;
  }

  goToHome() {
    this._router.navigate(['/']);
  }
}
