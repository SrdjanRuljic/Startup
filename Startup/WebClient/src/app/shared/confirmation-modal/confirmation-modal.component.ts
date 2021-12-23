import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-confirmation-modal',
  templateUrl: './confirmation-modal.component.html',
  styleUrls: ['./confirmation-modal.component.scss'],
})
export class ConfirmationModalComponent implements OnInit {
  public onClose: Subject<boolean>;

  constructor(private _bsModalRef: BsModalRef) {
    this.onClose = new Subject();
  }

  ngOnInit() {}

  public confirm(): void {
    this.onClose.next(true);
    this._bsModalRef.hide();
  }

  public decline(): void {
    this.onClose.next(false);
    this._bsModalRef.hide();
  }
}
