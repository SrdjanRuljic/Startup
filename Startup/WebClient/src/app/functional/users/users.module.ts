import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { UsersListComponent } from './users-list/users-list.component';
import { UsersFormComponent } from './users-form/users-form.component';
import { UpdateSelfComponent } from './update-self/update-self.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ChangeUserPasswordComponent } from './change-user-password/change-user-password.component';

import { UsersRoutingModule } from './users-routing.module';

@NgModule({
  imports: [CommonModule, UsersRoutingModule, FormsModule, PaginationModule],
  declarations: [
    UsersListComponent,
    UsersFormComponent,
    UpdateSelfComponent,
    ChangePasswordComponent,
    ChangeUserPasswordComponent,
  ],
})
export class UsersModule {}
