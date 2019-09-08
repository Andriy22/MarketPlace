import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';
import { MessageModel } from 'src/app/shared/models/MessageModel';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { API } from 'src/app/shared/config';

@Component({
  selector: 'app-chatforuser',
  templateUrl: './chatforuser.component.html',
  styleUrls: ['./chatforuser.component.css']
})
export class ChatforuserComponent implements OnInit {

  private _hubConnection: HubConnection;
  username: BehaviorSubject<string>;
  messages: MessageModel[] = [];
  message =  '';
  users: string[] = ['vasyan', 'petya'];
  constructor(public aS: AuthenticationService) {
    this.username = new BehaviorSubject<string>(null);
  }

  ngOnInit(): void {
    this._hubConnection = new HubConnectionBuilder().withUrl(API + '/chat', {
      accessTokenFactory: () => {
        return window.localStorage.getItem('access_token');
      }
     },
    ).build();
    this._hubConnection
      .start().then(() => {
        this._hubConnection.invoke('GetUsersForSendMeMsg');
        this.username.subscribe((x) => {
          this._hubConnection.invoke('GetMessagesToMe', x);
        });
      })
      .catch(err => console.log('Error while establishing connection :('));
    this._hubConnection.on('getUsersForSendMe', (users: string[]) => {
        this.users = users;
        // const chatElement = document.querySelector('.chat');
        // setTimeout(() => {
        //  chatElement.scrollTop = chatElement.scrollHeight;
        // }, 0);
      });
    this._hubConnection.on('reciveMessegesUser', (msgs: MessageModel[]) => {
        this.messages = msgs;
        const chatElement = document.querySelector('.chat');
        setTimeout(() => {
         chatElement.scrollTop = chatElement.scrollHeight;
        }, 0);
      });

    this._hubConnection.on('reciveMessegeFromUser', (msg: MessageModel) => {
        if (msg.toname === this.username.value || msg.nickname === this.username.value) {
          this.messages.push(msg);
        } else {
          const index = this.users.indexOf(msg.nickname);
          if (index === -1) {
            this.users.unshift(msg.nickname);
          }
        }
        // this.messages.push(msg);
        const chatElement = document.querySelector('.chat');
        setTimeout(() => {
         chatElement.scrollTop = chatElement.scrollHeight;
        }, 0);
   });

  }

  SetUserName(username: string) {
    this.username.next(username);
  }

  Send() {
    // this.id.subscribe((id) => {
    //   this._hubConnection.invoke('SendAllMessages', id);
    // });
    this._hubConnection.invoke('SendMessageToUser', this.username.value, this.message);
    this.message = '';
  }
}
