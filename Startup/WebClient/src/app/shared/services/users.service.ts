import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { AppGlobals } from 'src/app/core/app-globals';
import { IUser, IUserWithRoles } from 'src/app/shared/models/user';
import { IChangePassword } from '../models/change-password';
import { IChangeUserPassword } from '../models/change-user-password';

const DISPLAYNAME_KEY = 'display-name';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private _usersUrl = this._appGlobals.WebApiUrl + 'api/users';
  displayName$ = new BehaviorSubject<string>(this.displayName);

  constructor(private _appGlobals: AppGlobals, private _http: HttpClient) {}

  search(model: any): Observable<any> {
    return this._http
      .post(this._usersUrl + '/' + 'search', model)
      .pipe(map((res) => res));
  }

  insert(model: IUserWithRoles): Observable<any> {
    return this._http.post(this._usersUrl, model).pipe(map((res) => res));
  }

  getById(id: string): Observable<any> {
    return this._http.get(this._usersUrl + '/' + id).pipe(map((res) => res));
  }

  getLoggedIn(): Observable<any> {
    return this._http.get(this._usersUrl + '/loggedin').pipe(map((res) => res));
  }

  update(model: IUserWithRoles): Observable<any> {
    return this._http.put(this._usersUrl, model).pipe(map((res) => res));
  }

  updateSelf(model: IUser): Observable<any> {
    return this._http
      .put(this._usersUrl + '/self', model)
      .pipe(map((res) => res));
  }

  delete(id: string): Observable<any> {
    return this._http.delete(this._usersUrl + '/' + id).pipe(map((res) => res));
  }

  getDisplayName(): Observable<any> {
    return this._http.get(this._usersUrl + '/display-name').pipe(
      map((res: any) => {
        this.displayName = res.displayName;
      })
    );
  }

  changePassword(model: IChangePassword): Observable<any> {
    return this._http
      .put(this._usersUrl + '/change-password', model)
      .pipe(map((res) => res));
  }

  changeUserPassword(model: IChangeUserPassword): Observable<any> {
    return this._http
      .put(this._usersUrl + '/change-user-password', model)
      .pipe(map((res) => res));
  }

  set displayName(value: any) {
    if (value === '') {
      localStorage.removeItem(DISPLAYNAME_KEY);
    } else {
      this.displayName$.next(value);
      localStorage.setItem(DISPLAYNAME_KEY, value);
    }
  }

  get displayName() {
    return localStorage.getItem(DISPLAYNAME_KEY);
  }
}
