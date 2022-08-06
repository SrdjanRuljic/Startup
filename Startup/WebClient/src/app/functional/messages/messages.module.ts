import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TimeagoModule } from 'ngx-timeago';
//import { TimeAgoExtPipe } from 'src/app/shared/pipes/time-ago-ext.pipe';

import { MessagesListComponent } from './messages-list/messages-list.component';

import { MessagesRoutingModule } from './messages-routing.module';
import { InterlocutorsListComponent } from './interlocutors-list/interlocutors-list.component';

@NgModule({
  imports: [
    CommonModule,
    MessagesRoutingModule,
    PaginationModule,
    FormsModule,
    TimeagoModule,
  ],
  declarations: [
    MessagesListComponent,
    InterlocutorsListComponent /*TimeAgoExtPipe*/,
  ],
})
export class MessagesModule {}
