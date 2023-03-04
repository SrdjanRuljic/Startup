import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IMessage } from 'src/app/shared/models/message';
import { AuthService } from 'src/app/shared/services/auth.service';
import { MessagesService } from 'src/app/shared/services/messages.service';
import { TokenService } from 'src/app/shared/services/token.service';

@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss'],
})
export class ConversationComponent implements OnInit, OnDestroy {
  @ViewChild('f', { static: false }) form!: NgForm;
  username: string = '';
  model: IMessage;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    public _messagesService: MessagesService,
    private _tokenService: TokenService
  ) {
    this.model = {
      content: '',
      recipientUserName: '',
    };
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnDestroy(): void {
    this._messagesService.stopHubConnection();
  }

  ngOnInit() {
    this._route.params.subscribe((params) => {
      this.username = params['username'];
      if (this.username != '0') {
        const token = this._tokenService.getToken();
        this.model.recipientUserName = this.username;

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

  save() {
    if (this.contentValidation()) {
      this.sendMessage();
    }
  }

  sendMessage() {
    this._messagesService.sendMessage(this.model).then(() => {
      this.form.resetForm();
    });
  }

  contentValidation() {
    return !!!(this.model.content === '');
  }
}
