import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, take } from 'rxjs';
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
    private _toastrService: ToastrService,
    private _router: Router
  ) {}

  createHubConnection(token: any) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this._presenceHubUrl, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start();

    this.hubConnection.on('UserIsOnline', (id) => {
      this.onlineUsers$.pipe(take(1)).subscribe((users) => {
        this.onlineUsersSources.next([...users, id]);
      });
    });

    this.hubConnection.on('UserIsOffline', (id) => {
      this.onlineUsers$.pipe(take(1)).subscribe((users) => {
        this.onlineUsersSources.next([...users.filter((x) => x !== id)]);
      });
    });

    this.hubConnection.on('GetOnlineUsers', (users: string[]) => {
      this.onlineUsersSources.next(users);
    });

    this.hubConnection.on('NewMessageRecived', (id) => {
      this._toastrService
        .info('You have new message!')
        .onTap.pipe(take(1))
        .subscribe(() => this._router.navigateByUrl('/conversation/' + id));
    });
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }
}
