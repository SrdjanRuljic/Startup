import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UsersListComponent } from './users-list/users-list.component';
import { UsersFormComponent } from './users-form/users-form.component';
import { UpdateSelfComponent } from './update-self/update-self.component';
import { ChangePasswordComponent } from './change-password/change-password.component';

import { AuthGuard } from 'src/app/core/auth.guard';
import { PERMISSION } from 'src/app/shared/enums/permissions';

const userRoutes: Routes = [
  {
    path: 'users',
    component: UsersListComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
    data: { permission: PERMISSION.ADMIN },
  },
  {
    path: 'users/form/:id',
    component: UsersFormComponent,
    canActivate: [AuthGuard],
    data: { permission: PERMISSION.ADMIN },
  },
  {
    path: 'users/update-self',
    component: UpdateSelfComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'users/change-password',
    component: ChangePasswordComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forChild(userRoutes)],
  exports: [RouterModule],
})
export class UsersRoutingModule {}
