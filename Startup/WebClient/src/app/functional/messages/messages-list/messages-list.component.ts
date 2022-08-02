import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MessagesSearchModel } from 'src/app/shared/models/messages-search';
import { IPagination } from 'src/app/shared/models/pagination';
import { ConfirmationModalService } from 'src/app/shared/services/confirmation-modal.service';
import { MessagesService } from 'src/app/shared/services/messages.service';

@Component({
  selector: 'app-messages-list',
  templateUrl: './messages-list.component.html',
  styleUrls: ['./messages-list.component.scss'],
})
export class MessagesListComponent implements OnInit {
  messages: any[];
  searchModel: MessagesSearchModel;
  pagination: IPagination;

  constructor(
    private _router: Router,
    private _toastrService: ToastrService,
    private _messagesService: MessagesService,
    private _confirmationModalService: ConfirmationModalService
  ) {
    this.messages = [];
    this.searchModel = new MessagesSearchModel(1, 10, '');
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
    this.searchModel.container = 'Unread';
    this.searchModel.pageNumber = 1;
    this.searchModel.pageSize = 10;
  }

  search() {
    this._messagesService.search(this.searchModel).subscribe((response) => {
      this.messages = response.list;
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
    this.searchModel.pageNumber = event.page;
    this.search();
  }

  onSearchChange() {
    if (this.searchModel.container.length > 1) {
      this.search();
    } else if (this.searchModel.container.length == 0) {
      this.resetSearch();
    }
  }
}
