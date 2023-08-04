import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/core/auth.guard';

import { MessagesListComponent } from './messages-list/messages-list.component';
import { ConversationsComponent } from './conversations/conversations.component';

const messageRoutes: Routes = [
  {
    path: 'messages',
    component: MessagesListComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
  },
  {
    path: 'conversations',
    component: ConversationsComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(messageRoutes)],
  exports: [RouterModule],
})
export class MessagesRoutingModule {}
