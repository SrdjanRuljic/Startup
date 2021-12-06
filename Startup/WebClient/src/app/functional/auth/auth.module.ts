import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { RegisterComponent } from './register/register.component';

import { AuthRoutingModule } from './auth-routing.module';

@NgModule({
  imports: [CommonModule, AuthRoutingModule, FormsModule],
  declarations: [RegisterComponent],
  exports: [RegisterComponent],
})
export class AuthModule {}
