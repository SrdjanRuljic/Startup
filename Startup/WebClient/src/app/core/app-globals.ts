import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable()
export class AppGlobals {
  WebApiUrl: string = environment.api_url;
  HubUrl: string = environment.hub_url;
}
