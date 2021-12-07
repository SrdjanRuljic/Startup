import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss'],
})
export class MenuComponent implements OnInit {
  isAuthorized$: Observable<boolean>;
  modalRef?: BsModalRef;

  constructor(
    private _router: Router,
    private _authService: AuthService,
    private _modalService: BsModalService
  ) {
    this.isAuthorized$ = this._authService.getIsAuthorized();
  }

  ngOnInit() {}

  logout(template: TemplateRef<any>) {
    this.modalRef = this._modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {
    this._authService.logout();
    this.modalRef?.hide();
    this.goToHome();
  }

  decline(): void {
    this.modalRef?.hide();
  }

  goToHome() {
    this._router.navigate(['/']);
  }

  goToLogin() {
    this._router.navigate(['/login']);
  }

  goToRegister() {
    this._router.navigate(['/register']);
  }
}
