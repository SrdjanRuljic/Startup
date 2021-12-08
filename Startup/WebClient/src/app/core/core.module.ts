import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppGlobals } from './app-globals';
import { ErrorInterceptorProvider } from './http-interceptor';
import { JwtInterceptorProvider } from './jwt-interceptor';

@NgModule({
  imports: [CommonModule],
  declarations: [],
  providers: [AppGlobals, ErrorInterceptorProvider, JwtInterceptorProvider],
})
export class CoreModule {}
