import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthModule } from './auth/auth.module';
import { UsersModule } from './users/users.module';
import { MessagesModule } from './messages/messages.module';

@NgModule({
  imports: [CommonModule, AuthModule, UsersModule, MessagesModule],
  declarations: [],
})
export class FunctionalModule {}
