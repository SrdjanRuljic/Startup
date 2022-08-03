import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TimeagoModule } from 'ngx-timeago';

import { MessagesListComponent } from './messages-list/messages-list.component';

import { MessagesRoutingModule } from './messages-routing.module';
//import { TimeAgoExtPipe } from 'src/app/shared/pipes/time-ago-ext.pipe';

@NgModule({
  imports: [
    CommonModule,
    MessagesRoutingModule,
    PaginationModule,
    FormsModule,
    TimeagoModule,
  ],
  declarations: [MessagesListComponent /*TimeAgoExtPipe*/],
})
export class MessagesModule {}
