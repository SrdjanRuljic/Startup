import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { LoaderService } from '../services/loader.service';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss'],
  encapsulation: ViewEncapsulation.ShadowDom,
})
export class SpinnerComponent implements OnInit {
  constructor(private _loaderService: LoaderService) {}

  ngOnInit() {}

  getLoading(): boolean {
    return this._loaderService.getLoading();
  }
}
