import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationModalService } from 'src/app/shared/services/confirmation-modal.service';
import { MessagesService } from 'src/app/shared/services/messages.service';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss'],
})
export class ConversationComponent implements OnInit {
  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _toastrService: ToastrService,
    private _messagesService: MessagesService,
    private _confirmationModalService: ConfirmationModalService
  ) {}

  ngOnInit() {
    this._route.params.subscribe((params) => {
      let username = params['username'];
      if (username != '0') {
        console.log(username);
      }
    });
  }

  goToInterlocutors() {
    this._router.navigate(['/interlocutors']);
  }
}
