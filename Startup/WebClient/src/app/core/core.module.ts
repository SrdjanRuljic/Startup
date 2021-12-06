import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppGlobals } from './app-globals';
import { ErrorInterceptorProvider } from './http-intercepter';

@NgModule({
  imports: [CommonModule],
  declarations: [],
  providers: [AppGlobals, ErrorInterceptorProvider],
})
export class CoreModule {}
