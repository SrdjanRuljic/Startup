import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HTTP_INTERCEPTORS,
  HttpErrorResponse,
} from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../shared/services/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
    null
  );

  constructor(
    private _router: Router,
    private _toastrService: ToastrService,
    private _authService: AuthService
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error) => {
        if (error.status === 401) {
          this.handleUnauthorized(request, next);
        }
        if (error instanceof HttpErrorResponse) {
          this.handleError(error.error);
        }

        return throwError(() => error);
      })
    );
  }

  handleError(message: any) {
    this._toastrService.error(message);
  }

  handleUnauthorized(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      if (this._authService.getToken()) {
        this._authService
          .refreshToken()
          .pipe(
            switchMap((response: any) => {
              this.isRefreshing = false;
              this.refreshTokenSubject.next(response.auth_token);

              this._authService.handleRefreshSuccess(response);

              return next.handle(
                this.refreshTokenHeader(request, response.auth_token)
              );
            }),
            catchError((error) => {
              this.isRefreshing = false;

              this.goToHome();
              this._authService.logout();

              return throwError(() => error);
            })
          )
          .subscribe();
      }
    }

    return this.refreshTokenSubject.pipe(
      filter((token) => token !== null),
      take(1),
      switchMap((token) => next.handle(this.refreshTokenHeader(request, token)))
    );
  }

  goToHome() {
    this._router.navigate(['/']);
  }

  private refreshTokenHeader(request: HttpRequest<any>, token: string) {
    return request.clone({
      setHeaders: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });
  }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true,
};
