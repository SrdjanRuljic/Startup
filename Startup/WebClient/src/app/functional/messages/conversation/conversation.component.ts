import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IMessage } from 'src/app/shared/models/message';
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
  model: IMessage;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _toastrService: ToastrService,
    private _messagesService: MessagesService,
    private _confirmationModalService: ConfirmationModalService
  ) {
    this.messages = [];
    this.model = {
      content: '',
      recipientUserName: '',
    };
  }

  ngOnInit() {
    this._route.params.subscribe((params) => {
      this.username = params['username'];
      if (this.username != '0') {
        this.getThread();
        this.model.recipientUserName = this.username;
      }
    });
  }

  goToInterlocutors() {
    this._router.navigate(['/interlocutors']);
  }

  getThread() {
    this._messagesService.thread(this.username).subscribe((response) => {
      this.messages = response;
    });
  }

  save() {
    if (this.contentValidation()) {
      this.insert();
    }
  }

  insert() {
    this._messagesService.insert(this.model).subscribe((response) => {
      this.model.content = '';
      this.getThread();
    });
  }

  contentValidation() {
    return !!!(this.model.content == '' || this.model.content.length < 1);
  }
}
