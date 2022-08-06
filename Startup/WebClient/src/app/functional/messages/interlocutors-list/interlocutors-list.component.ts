import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { InterlocutorSearchModel } from 'src/app/shared/models/interlocutors-search';
import { IPagination } from 'src/app/shared/models/pagination';
import { ConfirmationModalService } from 'src/app/shared/services/confirmation-modal.service';
import { MessagesService } from 'src/app/shared/services/messages.service';

@Component({
  selector: 'app-interlocutors-list',
  templateUrl: './interlocutors-list.component.html',
  styleUrls: ['./interlocutors-list.component.scss'],
})
export class InterlocutorsListComponent implements OnInit {
  interlocutors: any[];
  searchModel: InterlocutorSearchModel;
  pagination: IPagination;

  constructor(
    private _router: Router,
    private _toastrService: ToastrService,
    private _messagesService: MessagesService,
    private _confirmationModalService: ConfirmationModalService
  ) {
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

  resetSearch() {
    this.initSearchModel();
    this.search();
  }

  pageChanged(event: any) {
    if (this.searchModel.pageNumber !== event.page) {
      this.searchModel.pageNumber = event.page;
      this.search();
    }
  }

  onSearchChange() {
    if (this.searchModel.term.length > 1) {
      this.search();
    } else if (this.searchModel.term.length == 0) {
      this.resetSearch();
    }
  }

  goToConversation(username: string) {
    this._router.navigate(['/conversation', username]);
  }
}
