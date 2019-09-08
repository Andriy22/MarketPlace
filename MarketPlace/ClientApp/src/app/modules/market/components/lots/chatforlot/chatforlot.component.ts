import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { BehaviorSubject } from 'rxjs';
import { MessageModel } from 'src/app/shared/models/MessageModel';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { API } from 'src/app/shared/config';

@Component({
  selector: 'app-chatforlot',
  templateUrl: './chatforlot.component.html',
  styleUrls: ['./chatforlot.component.css']
})
export class ChatforlotComponent implements OnInit {

  private _hubConnection: HubConnection;
  username: BehaviorSubject<string>;
  messages: MessageModel[] = [];
  message =  '';
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
        this.username.subscribe((x) => {
          if (x != null) {
            this._hubConnection.invoke('GetMessagesToMe', x);
          }
        });
      })
      .catch(err => console.log('Error while establishing connection :('));

    this._hubConnection.on('reciveMessegesUser', (msgs: MessageModel[]) => {
        this.messages = msgs;
        const chatElement = document.querySelector('.chat');
        setTimeout(() => {
          if (chatElement) {
            chatElement.scrollTop = chatElement.scrollHeight;
          }

        }, 0);
      });

    this._hubConnection.on('reciveMessegeFromUser', (msg: MessageModel) => {
        this.messages.push(msg);
        const chatElement = document.querySelector('.chat');
        setTimeout(() => {
          if (chatElement) {
            chatElement.scrollTop = chatElement.scrollHeight;
          }

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
    if ( this.username.value != null) {
      this._hubConnection.invoke('SendMessageToUser', this.username.value, this.message);
      this.message = '';
    }
  }
}
