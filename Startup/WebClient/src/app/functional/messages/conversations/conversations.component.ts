import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { InterlocutorSearchModel } from 'src/app/shared/models/interlocutor-search';
import { IMessage } from 'src/app/shared/models/message';
import { IPagination } from 'src/app/shared/models/pagination';
import { MessagesService } from 'src/app/shared/services/messages.service';
import { TokenService } from 'src/app/shared/services/token.service';

@Component({
  selector: 'app-conversations',
  templateUrl: './conversations.component.html',
  styleUrls: ['./conversations.component.scss'],
})
export class ConversationsComponent implements OnInit {
  @ViewChild('f', { static: false }) form!: NgForm;
  model: IMessage;
  interlocutors: any[];
  searchModel: InterlocutorSearchModel;
  pagination: IPagination;
  selectedInterlocutor: any;

  constructor(
    public _messagesService: MessagesService,
    private _tokenService: TokenService
  ) {
    this.selectedInterlocutor = null;
    this.interlocutors = [];
    this.searchModel = new InterlocutorSearchModel(1, 10, '');
    this.pagination = {
      pageNumber: 0,
      totalCount: 0,
      totalPages: 0,
    };

    this.model = {
      content: '',
      recipientUserId: '',
    };
  }

  ngOnInit() {
    this.initSearchModel();
    this.search();
  }

  ngOnDestroy(): void {
    this._messagesService.stopHubConnection();
  }

  initSearchModel() {
    this.searchModel.term = '';
    this.searchModel.pageNumber = 1;
    this.searchModel.pageSize = 10;
  }

  search() {
    this._messagesService
      .searchInterlocutors(this.searchModel)
      .subscribe((response) => {
        this.interlocutors = response.list;

        this.pagination.pageNumber = response.pageNumber;
        this.pagination.totalCount = response.totalCount;
        this.pagination.totalPages = response.totalPages;
      });
  }

  goToConversation(interlocutor: any) {
    const token = this._tokenService.getToken();

    this.selectedInterlocutor = interlocutor;
    this.model.recipientUserId = this.selectedInterlocutor.id;

    this._messagesService.stopHubConnection();
    this._messagesService.createHubConnection(
      token as string,
      this.selectedInterlocutor?.id
    );
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
