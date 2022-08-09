import { Injectable } from '@angular/core';
import { AppGlobals } from 'src/app/core/app-globals';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable, take } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NgxPermissionsService } from 'ngx-permissions';
import { IRegister } from '../models/register';
import { ILogin } from '../models/login';
import { IConfirmEmail } from '../models/confirm-email';
import { IForgotPassword } from '../models/forgot-password';
import { IResetPassword } from '../models/reset-password';
import { PresenceService } from './presence.service';

const TOKEN_KEY = 'auth-token';
const PERMISSIONS_KEY = 'permissions';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _authUrl = this._appGlobals.WebApiUrl + 'auth';
  private _jwtHelper = new JwtHelperService();

  private isAuthorized$ = new BehaviorSubject<boolean>(this.getIsAuthorized());

  constructor(
    private _appGlobals: AppGlobals,
    private _http: HttpClient,
    private _permissionsService: NgxPermissionsService,
    private _presenceService: PresenceService
  ) {}

  register(model: IRegister): Observable<any> {
    return this._http
      .post(this._authUrl + '/' + 'register', model)
      .pipe(map((res) => res));
  }

  login(model: ILogin): Observable<any> {
    return this._http.post(this._authUrl + '/' + 'login', model).pipe(
      map((res) => {
        this.handleLoginSuccess(res);
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

  logout(): Observable<any> {
    return this._http.post(this._authUrl + '/' + 'logout', null).pipe(
      map((res) => {
        this.handleLogoutSuccess(res);
      })
    );
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

  private handleLoginSuccess(response: any) {
    this.saveToken(response.auth_token);
    this.isAuthorized$.next(true);

    this.getAllUserRoles();

    this._presenceService.createHubConnection(response.auth_token);
  }

  private handleLogoutSuccess(response: any) {
    this.isAuthorized$.next(false);
    localStorage.clear();

    this._permissionsService.flushPermissions();

    this._presenceService.stopHubConnection();
  }

  getIsAuthorized() {
    let token = this.getToken();
    return !this._jwtHelper.isTokenExpired(token!);
  }

  private getAllUserRoles() {
    this.getUserRoles()
      .pipe(take(1))
      .subscribe((response) => {
        localStorage.setItem(PERMISSIONS_KEY, JSON.stringify(response));
        this.loadPermissions();
      });
  }

  loadPermissions() {
    const permissions = localStorage.getItem(PERMISSIONS_KEY);

    if (permissions) {
      this._permissionsService.loadPermissions(JSON.parse(permissions));
    }
  }
}
