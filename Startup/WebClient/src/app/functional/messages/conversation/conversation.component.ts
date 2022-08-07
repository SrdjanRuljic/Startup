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
  username: string = '';
  messages: any[];

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _toastrService: ToastrService,
    private _messagesService: MessagesService,
    private _confirmationModalService: ConfirmationModalService
  ) {
    this.messages = [];
  }

  ngOnInit() {
    this._route.params.subscribe((params) => {
      this.username = params['username'];
      if (this.username != '0') {
        this.getThread(this.username);
      }
    });
  }

  goToInterlocutors() {
    this._router.navigate(['/interlocutors']);
  }

  getThread(username: string) {
    this._messagesService.thread(username).subscribe((response) => {
      this.messages = response;
    });
  }
}
