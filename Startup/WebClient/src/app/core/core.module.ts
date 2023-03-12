import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppGlobals } from './app-globals';
import { ErrorInterceptorProvider } from './http-interceptor';
import { JwtInterceptorProvider } from './jwt-interceptor';
import { LoadingInterceptorProvider } from './loading-interceptor';

@NgModule({
  imports: [CommonModule],
  declarations: [],
  providers: [
    AppGlobals,
    ErrorInterceptorProvider,
    JwtInterceptorProvider,
    LoadingInterceptorProvider,
  ],
})
export class CoreModule {}
