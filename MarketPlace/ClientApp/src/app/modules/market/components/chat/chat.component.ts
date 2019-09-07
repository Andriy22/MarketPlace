import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { MessageModel } from './../../../../shared/models/MessageModel';
import { Element } from '@angular/compiler/src/render3/r3_ast';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  private _hubConnection: HubConnection;
  messages: MessageModel[] = [{nickname: 'user', message: 'test', role: 'admin', ava: 'null', time: '24-08-2019' }];
  message =  '';
  constructor(protected aS: AuthenticationService) {
    this.messages.push({nickname: 'user', message: 'test', role: 'admin', ava: 'null', time: '24-08-2019' });
    this.message = '';
   }

  ngOnInit() {
    this._hubConnection = new HubConnectionBuilder().withUrl('https://localhost:44333/chat', {
      accessTokenFactory: () => {
        return window.localStorage.getItem('access_token');
      }
     },
    ).build();
    this._hubConnection
      .start()
      .catch(err => console.log('Error while establishing connection :('));
    console.log(this._hubConnection.state);

    this._hubConnection.on('reciveMessage', (nickname: string, message: string, role: string, ava: string, time: string) => {
         this.messages.push({nickname, message, role, ava, time});
         const chatElement = document.querySelector('.chat');
         setTimeout(() => {
          chatElement.scrollTop = chatElement.scrollHeight;
         }, 0);
    });
  }
  Send() {
    this._hubConnection.invoke('sendMessage', this.message );
    this.message = '';
  }

}
