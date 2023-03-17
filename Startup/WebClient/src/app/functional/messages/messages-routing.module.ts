import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/core/auth.guard';

import { MessagesListComponent } from './messages-list/messages-list.component';
import { InterlocutorsListComponent } from './interlocutors-list/interlocutors-list.component';
import { ConversationComponent } from './conversation/conversation.component';

const messageRoutes: Routes = [
  {
    path: 'messages',
    component: MessagesListComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
  },
  {
    path: 'interlocutors',
    component: InterlocutorsListComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
  },
  {
    path: 'conversation/:userId',
    component: ConversationComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(messageRoutes)],
  exports: [RouterModule],
})
export class MessagesRoutingModule {}
