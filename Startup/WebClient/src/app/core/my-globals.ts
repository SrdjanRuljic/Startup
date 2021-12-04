import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable()
export class MyGlobals {
  WebApiUrl: string = environment.api_url;
}
