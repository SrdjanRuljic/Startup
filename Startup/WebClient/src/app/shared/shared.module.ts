import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxNavbarModule } from 'ngx-bootstrap-navbar';
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';

import { AuthService } from './services/auth.service';

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    NgxNavbarModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
    }),
    HttpClientModule,
  ],
  declarations: [MenuComponent],
  exports: [MenuComponent],
  providers: [AuthService],
})
export class SharedModule {}
