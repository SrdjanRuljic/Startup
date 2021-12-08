import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { AppGlobals } from 'src/app/core/app-globals';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private _usersUrl = this._appGlobals.WebApiUrl + 'api/users';

  constructor(private _appGlobals: AppGlobals, private _http: HttpClient) {}

  search(model: any): Observable<any> {
    return this._http
      .post(this._usersUrl + '/' + 'search', model)
      .pipe(map((res) => res));
  }
}
