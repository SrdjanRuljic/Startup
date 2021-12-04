import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MyGlobals } from './my-globals';
import { ErrorInterceptorProvider } from './http-intercepter';

@NgModule({
  imports: [CommonModule],
  declarations: [],
  providers: [MyGlobals, ErrorInterceptorProvider],
})
export class CoreModule {}
