import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { InterlocutorSearchModel } from 'src/app/shared/models/interlocutor-search';
import { IPagination } from 'src/app/shared/models/pagination';
import { MessagesService } from 'src/app/shared/services/messages.service';

@Component({
  selector: 'app-conversations',
  templateUrl: './conversations.component.html',
  styleUrls: ['./conversations.component.scss'],
})
export class ConversationsComponent implements OnInit {
  interlocutors: any[];
  searchModel: InterlocutorSearchModel;
  pagination: IPagination;
  selectedInterlocutor: any;

  constructor(
    private _messagesService: MessagesService,
    private _router: Router
  ) {
    this.selectedInterlocutor = null;
    this.interlocutors = [];
    this.searchModel = new InterlocutorSearchModel(1, 10, '');
    this.pagination = {
      pageNumber: 0,
      totalCount: 0,
      totalPages: 0,
    };
  }

  ngOnInit() {
    this.initSearchModel();
    this.search();
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
    this.selectedInterlocutor = interlocutor;
    this._router.navigate(['/conversation', interlocutor.id]);
  }
}
