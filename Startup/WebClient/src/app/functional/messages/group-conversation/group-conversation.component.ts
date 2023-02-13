import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IChatGroup } from 'src/app/shared/models/chat-group';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ChatGroupService } from 'src/app/shared/services/chat-group.service';

@Component({
  selector: 'app-group-conversation',
  templateUrl: './group-conversation.component.html',
  styleUrls: ['./group-conversation.component.scss'],
})
export class GroupConversationComponent implements OnInit {
  @ViewChild('f', { static: false }) form!: NgForm;
  name: string = '';
  model: IChatGroup;

  constructor(
    private _router: Router,
    private _route: ActivatedRoute,
    private _authService: AuthService,
    public _chatGroupService: ChatGroupService
  ) {
    this.model = {
      content: '',
      groupName: '',
    };
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit() {
    this._route.params.subscribe((params) => {
      this.name = params['name'];
      if (this.name != '') {
        const token = this._authService.getToken();
      }
    });
  }

  goToInterlocutors() {
    this._router.navigate(['/interlocutors']);
  }

  contentValidation() {
    return !!!(this.model.content === '');
  }

  save() {
    if (this.contentValidation()) {
      this.sendMessage();
    }
  }

  sendMessage() {
    this._chatGroupService.sendMessage(this.model).then(() => {
      this.form.resetForm();
    });
  }
}
