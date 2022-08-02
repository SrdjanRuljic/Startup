import { ISearchModel } from 'src/app/shared/models/pagination';

export class MessagesSearchModel implements ISearchModel {
  pageNumber: number;
  pageSize: number;
  container: string;

  constructor(number: number, size: number, container: string) {
    this.pageNumber = number;
    this.pageSize = size;
    this.container = container;
  }
}
