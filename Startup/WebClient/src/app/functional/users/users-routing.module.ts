import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UsersListComponent } from './users-list/users-list.component';
// import { VidoesFormComponent } from './vidoes-form/vidoes-form.component';

import { AuthGuard } from 'src/app/core/auth.guard';
import { Role } from 'src/app/shared/enums/role';

const userRoutes: Routes = [
  {
    path: 'users',
    component: UsersListComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
    data: { role: Role.Admin },
  },
  // {
  //   path: 'videos/form/:id',
  //   component: VidoesFormComponent,
  //   canActivate: [AuthGuard],
  // },
];

@NgModule({
  imports: [RouterModule.forChild(userRoutes)],
  exports: [RouterModule],
})
export class UsersRoutingModule {}
