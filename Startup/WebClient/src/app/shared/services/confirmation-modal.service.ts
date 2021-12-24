import { Injectable } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationModalComponent } from '../confirmation-modal/confirmation-modal.component';

@Injectable()
export class ConfirmationModalService {
  constructor(private _bsModalService: BsModalService) {}

  confirm(message: string): Promise<boolean> {
    const modal = this._bsModalService.show(ConfirmationModalComponent, {
      class: 'modal-sm',
      initialState: { message: message },
    });

    return new Promise<boolean>((resolve, reject) =>
      modal.content?.result.subscribe((result: any) => resolve(result))
    );
  }
}
