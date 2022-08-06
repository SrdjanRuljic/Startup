import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { AppGlobals } from 'src/app/core/app-globals';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  private _messagesUrl = this._appGlobals.WebApiUrl + 'api/messages';

  constructor(private _appGlobals: AppGlobals, private _http: HttpClient) {}

  search(model: any): Observable<any> {
    return this._http
      .post(this._messagesUrl + '/' + 'search', model)
      .pipe(map((res) => res));
  }

  searchInterlocutors(model: any): Observable<any> {
    return this._http
      .post(this._messagesUrl + '/' + 'search-interlocutors', model)
      .pipe(map((res) => res));
  }
}
