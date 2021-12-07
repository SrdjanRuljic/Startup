import {
  HttpEvent,
  HttpHandler,
  HttpHeaders,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthService } from '../shared/services/auth.service';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: '',
  }),
};

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  isAuthorized$: Observable<boolean>;

  constructor(private _authService: AuthService) {
    this.isAuthorized$ = this._authService.getIsAuthorized();
  }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const isApiUrl = request.url.startsWith(environment.api_url);
    const isLoggedIn = this.isAuthorized$.pipe(take(1)) as unknown as boolean;

    if (isLoggedIn && isApiUrl) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this._authService.getToken}`,
        },
      });
    }

    return next.handle(request);
  }
}
