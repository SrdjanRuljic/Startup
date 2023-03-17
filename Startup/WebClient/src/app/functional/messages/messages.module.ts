import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TimeAgoExtPipe } from 'src/app/shared/pipes/time-ago-ext.pipe';

import { MessagesRoutingModule } from './messages-routing.module';

import { MessagesListComponent } from './messages-list/messages-list.component';
import { InterlocutorsListComponent } from './interlocutors-list/interlocutors-list.component';
import { ConversationComponent } from './conversation/conversation.component';

@NgModule({
  imports: [CommonModule, MessagesRoutingModule, PaginationModule, FormsModule],
  declarations: [
    MessagesListComponent,
    InterlocutorsListComponent,
    ConversationComponent,
    TimeAgoExtPipe,
  ],
})
export class MessagesModule {}
