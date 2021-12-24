import { Component, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-confirmation-modal',
  templateUrl: './confirmation-modal.component.html',
  styleUrls: ['./confirmation-modal.component.scss'],
})
export class ConfirmationModalComponent implements OnInit {
  @Input() message: string = '';
  result: Subject<boolean>;

  constructor(private _bsModalRef: BsModalRef) {
    this.result = new Subject();
  }

  ngOnInit() {}

  public confirm(): void {
    this.result.next(true);
    this._bsModalRef.hide();
  }

  public decline(): void {
    this.result.next(false);
    this._bsModalRef.hide();
  }
}
