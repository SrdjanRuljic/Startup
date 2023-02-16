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
import { catchError, switchMap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../shared/services/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  isRefreshing = false;

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
          this.handleUnauthorized();
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

  handleUnauthorized() {
    if (!this.isRefreshing) {
      this.isRefreshing = true;

      this._authService.refreshToken().subscribe((response) => {
        this.isRefreshing = false;
      });
    }
  }

  goToHome() {
    this._router.navigate(['/']);
  }
}

export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ErrorInterceptor,
  multi: true,
};
