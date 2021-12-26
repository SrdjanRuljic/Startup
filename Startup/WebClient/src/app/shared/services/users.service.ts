import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { AppGlobals } from 'src/app/core/app-globals';
import { IUser } from 'src/app/functional/users/user';

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

  insert(model: IUser): Observable<any> {
    return this._http.post(this._usersUrl, model).pipe(map((res) => res));
  }

  getById(id: string): Observable<any> {
    return this._http.get(this._usersUrl + '/' + id).pipe(map((res) => res));
  }

  update(model: IUser): Observable<any> {
    return this._http.put(this._usersUrl, model).pipe(map((res) => res));
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

  setDisplayName$(response: any | null) {
    if (response == null) {
      this.displayName$.next('');
    } else {
      this.displayName$.next(response.displayName);
    }
    console.log('this.displayName$:', this.displayName$);
  }
}
