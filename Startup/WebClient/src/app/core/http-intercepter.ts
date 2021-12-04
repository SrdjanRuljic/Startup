import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HTTP_INTERCEPTORS,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private _router: Router, private _toastrService: ToastrService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error) => {
        if (error.status === 401) {
          this.handleUnauthorized();
          return throwError(error.statusText);
        }
        if (error instanceof HttpErrorResponse) {
          this.handleError(error.error);
          return throwError(error.error);
        }
      })
    );
  }

  handleError(message) {
    this._toastrService.error(message);
  }

  handleUnauthorized() {
    this.goToHome();
    this._toastrService.error('You are unauthorized.');
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
