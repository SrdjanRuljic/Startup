import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TimeAgoExtPipe } from 'src/app/shared/pipes/time-ago-ext.pipe';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

import { MessagesRoutingModule } from './messages-routing.module';

import { MessagesListComponent } from './messages-list/messages-list.component';
import { ConversationsComponent } from './conversations/conversations.component';

@NgModule({
  imports: [
    CommonModule,
    MessagesRoutingModule,
    PaginationModule,
    FormsModule,
    InfiniteScrollModule,
  ],
  declarations: [MessagesListComponent, TimeAgoExtPipe, ConversationsComponent],
})
export class MessagesModule {}
