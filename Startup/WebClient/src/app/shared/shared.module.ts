import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxNavbarModule } from 'ngx-bootstrap-navbar';
import { ToastrModule } from 'ngx-toastr';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxPermissionsModule } from 'ngx-permissions';
import { ConfirmationModalModule } from './confirmation-modal/confirmation-modal.module';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TimeagoModule } from 'ngx-timeago';

import { AuthService } from './services/auth.service';
import { UsersService } from './services/users.service';
import { ConfirmationModalService } from './services/confirmation-modal.service';

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    NgxNavbarModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
    }),
    HttpClientModule,
    ModalModule.forRoot(),
    PaginationModule.forRoot(),
    NgxPermissionsModule.forRoot(),
    ConfirmationModalModule,
    BsDropdownModule.forRoot(),
    TimeagoModule.forRoot(),
  ],
  declarations: [MenuComponent],
  exports: [MenuComponent, NgxPermissionsModule, TimeagoModule],
  providers: [AuthService, UsersService, ConfirmationModalService],
})
export class SharedModule {}
