import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UsersListComponent } from './users-list/users-list.component';
import { UsersFormComponent } from './users-form/users-form.component';

import { AuthGuard } from 'src/app/core/auth.guard';
import { Permission } from 'src/app/shared/enums/permissions';

const userRoutes: Routes = [
  {
    path: 'users',
    component: UsersListComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
    data: { permission: Permission.Admin },
  },
  {
    path: 'users/form/:id',
    component: UsersFormComponent,
    canActivate: [AuthGuard],
    data: { permission: Permission.Admin },
  },
];

@NgModule({
  imports: [RouterModule.forChild(userRoutes)],
  exports: [RouterModule],
})
export class UsersRoutingModule {}
