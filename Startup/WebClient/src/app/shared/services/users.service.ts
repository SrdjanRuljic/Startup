import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { AppGlobals } from 'src/app/core/app-globals';
import { IChangePassword } from 'src/app/functional/users/change-password';
import { IUser, IUserWithRoles } from 'src/app/functional/users/user';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private _usersUrl = this._appGlobals.WebApiUrl + 'api/users';
  private displayName$ = new BehaviorSubject<string>('');

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

  getByUserName(): Observable<any> {
    return this._http
      .get(this._usersUrl + '/get-by-username')
      .pipe(map((res) => res));
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
      map((res) => {
        this.setDisplayName$(res);
      })
    );
  }

  changePassword(model: IChangePassword): Observable<any> {
    return this._http
      .put(this._usersUrl + '/change-password', model)
      .pipe(map((res) => res));
  }

  getDisplayName$(): Observable<string> {
    return this.displayName$.asObservable();
  }

  setDisplayName$(response: any | null) {
    if (response == null) {
      this.displayName$.next('');
    } else {
      this.displayName$.next(response.displayName);
    }
  }
}
