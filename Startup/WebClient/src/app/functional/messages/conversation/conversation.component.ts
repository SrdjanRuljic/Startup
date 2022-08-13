import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IMessage } from 'src/app/shared/models/message';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ConfirmationModalService } from 'src/app/shared/services/confirmation-modal.service';
import { MessagesService } from 'src/app/shared/services/messages.service';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss'],
})
export class ConversationComponent implements OnInit, OnDestroy {
  username: string = '';
  messages: any[];
  model: IMessage;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _toastrService: ToastrService,
    public _messagesService: MessagesService,
    private _confirmationModalService: ConfirmationModalService,
    private _authService: AuthService
  ) {
    this.messages = [];
    this.model = {
      content: '',
      recipientUserName: '',
    };
  }

  ngOnDestroy(): void {
    this._messagesService.stopHubConnection();
  }

  ngOnInit() {
    this._route.params.subscribe((params) => {
      this.username = params['username'];
      if (this.username != '0') {
        const token = this._authService.getToken();
        this.model.recipientUserName = this.username;

        this.getThread();
        this._messagesService.createHubConnection(
          token as string,
          this.username
        );
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
