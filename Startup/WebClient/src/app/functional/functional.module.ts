import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthModule } from './auth/auth.module';
import { UsersModule } from './users/users.module';

@NgModule({
  imports: [CommonModule, AuthModule, UsersModule],
  declarations: [],
})
export class FunctionalModule {}
