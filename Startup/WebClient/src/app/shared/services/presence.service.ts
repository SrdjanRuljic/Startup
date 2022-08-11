import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { AppGlobals } from 'src/app/core/app-globals';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  private _presenceHubUrl = this._appGlobals.HubUrl + 'presence';
  private hubConnection!: HubConnection;
  private onlineUsersSources = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSources.asObservable();

  constructor(
    private _appGlobals: AppGlobals,
    private _toastrService: ToastrService
  ) {}

  createHubConnection(token: any) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this._presenceHubUrl, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start();

    this.hubConnection.on('UserIsOnline', (username) => {
      this._toastrService.info(username + ' has connected');
    });

    this.hubConnection.on('UserIsOffline', (username) => {
      this._toastrService.error(username + ' has disconnected');
    });

    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.onlineUsersSources.next(usernames);
    });
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }
}
