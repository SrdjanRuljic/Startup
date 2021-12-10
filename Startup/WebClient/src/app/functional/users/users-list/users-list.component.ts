import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IPagination, ISearchModel } from 'src/app/shared/models/pagination';
import { AuthService } from 'src/app/shared/services/auth.service';
import { UsersService } from 'src/app/shared/services/users.service';
import { UsersSearchModel } from '../users-search';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss'],
})
export class UsersListComponent implements OnInit {
  users: any[];
  searchModel: UsersSearchModel;
  pagination: IPagination;

  constructor(
    private _router: Router,
    private _authService: AuthService,
    private _toastrService: ToastrService,
    private _usersService: UsersService
  ) {
    this.users = [];
    this.searchModel = new UsersSearchModel(1, 10, '');
    this.pagination = {
      pageNumber: 0,
      totalCount: 0,
      totalPages: 0,
    };
  }

  initSearchModel() {
    this.searchModel.term = '';
    this.searchModel.pageNumber = 1;
    this.searchModel.pageSize = 10;
  }

  ngOnInit() {
    this.initSearchModel();
    this.search();
  }

  search() {
    this._usersService.search(this.searchModel).subscribe((response) => {
      this.users = response.list;
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
    if (this.searchModel.term.length > 1) {
      this.search();
    } else if (this.searchModel.term.length == 0) {
      this.resetSearch();
    }
  }

  goToVideoForm(id: any) {
    this._router.navigate(['/users/form', id]);
  }
}
