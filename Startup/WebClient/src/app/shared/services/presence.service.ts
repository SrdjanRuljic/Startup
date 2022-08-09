import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { AppGlobals } from 'src/app/core/app-globals';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  private _presenceHubUrl = this._appGlobals.HubUrl + 'presence';
  private hubConnection!: HubConnection;

  constructor(
    private _appGlobals: AppGlobals,
    private _toastrService: ToastrService
  ) {}

  createHubConnection(token: any) {
    this.hubConnection = new HubConnectionBuilder()
      .configureLogging(signalR.LogLevel.Debug)
      .withUrl(this._presenceHubUrl, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('UserIsOnline', (username) => {
      this._toastrService.info(username + 'has connected');
    });

    this.hubConnection.on('UserIsOffline', (username) => {
      this._toastrService.error(username + 'has disconnected');
    });
  }

  stopHubConnection() {
    this.hubConnection.stop().catch((error) => console.log(error));
  }
}
