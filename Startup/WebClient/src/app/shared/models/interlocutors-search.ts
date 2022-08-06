import { ISearchModel } from 'src/app/shared/models/pagination';

export class InterlocutorSearchModel implements ISearchModel {
  pageNumber: number;
  pageSize: number;
  term: string;

  constructor(number: number, size: number, term: string) {
    this.pageNumber = number;
    this.pageSize = size;
    this.term = term;
  }
}
