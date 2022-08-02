import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { MessagesListComponent } from './messages-list/messages-list.component';

import { MessagesRoutingModule } from './messages-routing.module';

@NgModule({
  imports: [CommonModule, MessagesRoutingModule, PaginationModule, FormsModule],
  declarations: [MessagesListComponent],
})
export class MessagesModule {}
