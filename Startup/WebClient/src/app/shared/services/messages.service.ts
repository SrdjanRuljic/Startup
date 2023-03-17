import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, map, Observable, take } from 'rxjs';
import { AppGlobals } from 'src/app/core/app-globals';
import { IGroup } from '../models/group';
import { IMessage } from '../models/message';

@Injectable({
  providedIn: 'root',
})
export class MessagesService {
  private _messagesUrl = this._appGlobals.WebApiUrl + 'messages';
  private _messageHubUrl = this._appGlobals.HubUrl + 'message';
  private hubConnection!: HubConnection;
  private messagesThreadSources = new BehaviorSubject<any[]>([]);
  messagesThread$ = this.messagesThreadSources.asObservable();

  constructor(private _appGlobals: AppGlobals, private _http: HttpClient) {}

  createHubConnection(token: string, recipientUserId: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this._messageHubUrl + '?recipient=' + recipientUserId, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start();

    this.hubConnection.on('ReciveMessageThread', (messages) => {
      this.messagesThreadSources.next(messages);
    });

    this.hubConnection.on('NewMessage', (message) => {
      this.messagesThread$.pipe(take(1)).subscribe((messages) => {
        this.messagesThreadSources.next([...messages, message]);
      });
    });

    this.hubConnection.on('UpdatedGroup', (group: IGroup) => {
      if (group.connections.some((x) => x.userId === recipientUserId)) {
        this.messagesThread$.pipe(take(1)).subscribe((messages) => {
          messages.forEach((message) => {
            if (!message.dateRead) {
              message.dateRead = new Date(Date.now());
            }
          });
          this.messagesThreadSources.next([...messages]);
        });
      }
    });
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.messagesThreadSources.next([]);
      this.hubConnection.stop();
    }
  }

  search(model: any): Observable<any> {
    return this._http
      .post(this._messagesUrl + '/' + 'search', model)
      .pipe(map((res) => res));
  }

  searchInterlocutors(model: any): Observable<any> {
    return this._http
      .post(this._messagesUrl + '/' + 'search-interlocutors', model)
      .pipe(map((res) => res));
  }

  async sendMessage(model: IMessage) {
    return this.hubConnection.invoke(
      'SendMessage',
      model.recipientUserId,
      model.content
    );
  }

  delete(id: number): Observable<any> {
    return this._http
      .delete(this._messagesUrl + '/' + id)
      .pipe(map((res) => res));
  }
}
