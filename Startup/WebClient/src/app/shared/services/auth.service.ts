import { Injectable } from '@angular/core';
import { AppGlobals } from 'src/app/core/app-globals';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable, take } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxPermissionsService } from 'ngx-permissions';
import { PERMISSION } from '../enums/permissions';
import { IForgotPassword } from 'src/app/functional/auth/forgot-password';
import { IResetPassword } from 'src/app/functional/auth/reset-password';
import { IConfirmEmail } from 'src/app/functional/auth/confirm-email';

const TOKEN_KEY = 'auth-token';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _authUrl = this._appGlobals.WebApiUrl + 'api/auth';
  private _jwtHelper = new JwtHelperService();

  private isAuthorized$ = new BehaviorSubject<boolean>(this.getIsAuthorized());

  constructor(
    private _appGlobals: AppGlobals,
    private _http: HttpClient,
    private _permissionsService: NgxPermissionsService
  ) {}

  register(model: any): Observable<any> {
    return this._http
      .post(this._authUrl + '/' + 'register', model)
      .pipe(map((res) => res));
  }

  login(model: any): Observable<any> {
    return this._http.post(this._authUrl + '/' + 'login', model).pipe(
      map((res) => {
        this.handleSuccess(res);
      })
    );
  }

  confirmEmail(model: IConfirmEmail): Observable<any> {
    return this._http
      .post(this._authUrl + '/' + 'confirm-email', model)
      .pipe(map((res) => res));
  }

  forgotPassword(model: IForgotPassword): Observable<any> {
    return this._http
      .post(this._authUrl + '/' + 'forgot-password', model)
      .pipe(map((res) => res));
  }

  resetPassword(model: IResetPassword): Observable<any> {
    return this._http
      .post(this._authUrl + '/' + 'reset-password', model)
      .pipe(map((res) => res));
  }

  getUserRoles(): Observable<any> {
    return this._http
      .get(this._authUrl + '/user-roles')
      .pipe(map((res) => res));
  }

  logout(): void {
    this.isAuthorized$.next(false);
    localStorage.clear();

    this._permissionsService.flushPermissions();
  }

  getIsAuthorized$(): Observable<boolean> {
    return this.isAuthorized$.asObservable();
  }

  getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  private saveToken(token: string): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.setItem(TOKEN_KEY, token);
  }

  private handleSuccess(response: any) {
    this.saveToken(response.auth_token);
    this.isAuthorized$.next(true);

    this.loadPermissions();
  }

  getIsAuthorized() {
    let token = this.getToken();
    return !this._jwtHelper.isTokenExpired(token!);
  }

  private loadPermissions() {
    this.getUserRoles()
      .pipe(take(1))
      .subscribe((response) => {
        this._permissionsService.loadPermissions(response);
      });
  }
}
