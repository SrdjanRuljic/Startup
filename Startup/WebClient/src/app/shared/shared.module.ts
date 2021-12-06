import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu/menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxNavbarModule } from 'ngx-bootstrap-navbar';

@NgModule({
  imports: [CommonModule, BrowserAnimationsModule, NgxNavbarModule],
  declarations: [MenuComponent],
  exports: [MenuComponent],
})
export class SharedModule {}
