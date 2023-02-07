import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { AppGlobals } from 'src/app/core/app-globals';
import { IChatGroup } from '../models/chat-group';
import { IGroup } from '../models/group';

@Injectable({
  providedIn: 'root',
})
export class ChatGroupService {
  private _chatGroupHubUrl = this._appGlobals.HubUrl + 'chat-group';
  private hubConnection!: HubConnection;
  private messagesThreadSources = new BehaviorSubject<any[]>([]);
  messagesThread$ = this.messagesThreadSources.asObservable();

  constructor(private _appGlobals: AppGlobals, private _http: HttpClient) {}

  createHubConnection(
    token: string,
    groupName: string,
    recipientUserName: string
  ) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this._chatGroupHubUrl + '?group=' + groupName, {
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
      if (group.connections.some((x) => x.username === recipientUserName)) {
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
      this.hubConnection.stop();
    }
  }

  async sendMessage(model: IChatGroup) {
    return this.hubConnection.invoke(
      'SendMessage',
      model.groupName,
      model.content
    );
  }
}
