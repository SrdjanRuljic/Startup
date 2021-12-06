import { Injectable } from '@angular/core';
import { AppGlobals } from 'src/app/core/app-globals';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private _authUrl = this._appGlobals.WebApiUrl + 'api/auth';

  constructor(private _appGlobals: AppGlobals, private _http: HttpClient) {}

  register(model: any): Observable<any> {
    return this._http
      .post(this._authUrl + '/' + 'register', model)
      .pipe(map((res) => res));
  }
}
