import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { UsersListComponent } from './users-list/users-list.component';

import { UsersRoutingModule } from './users-routing.module';

@NgModule({
  imports: [CommonModule, UsersRoutingModule, FormsModule, PaginationModule],
  declarations: [UsersListComponent],
})
export class UsersModule {}
