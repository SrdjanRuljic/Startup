import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { UsersListComponent } from './users-list/users-list.component';
import { UsersFormComponent } from './users-form/users-form.component';
import { UpdateSelfComponent } from './update-self/update-self.component';

import { UsersRoutingModule } from './users-routing.module';

@NgModule({
  imports: [CommonModule, UsersRoutingModule, FormsModule, PaginationModule],
  declarations: [UsersListComponent, UsersFormComponent, UpdateSelfComponent],
})
export class UsersModule {}
