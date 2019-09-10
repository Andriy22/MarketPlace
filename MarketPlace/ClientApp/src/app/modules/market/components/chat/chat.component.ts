import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { MessageModel } from './../../../../shared/models/MessageModel';
import { Element } from '@angular/compiler/src/render3/r3_ast';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { API } from './../../../../shared/config';
import { BehaviorSubject } from 'rxjs';
import { ErrorHandler } from 'src/app/shared/helpers/ErrorHandler';
import { Router } from '@angular/router';
import { NzNotificationService } from 'ng-zorro-antd';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  private _hubConnection: HubConnection;
  id: BehaviorSubject<string>;
  messages: MessageModel[] = [];
  message =  '';
  constructor(protected aS: AuthenticationService, private router: Router, private notify: NzNotificationService) {
    this.id = new BehaviorSubject<string>('0');
    this.message = '';

   }

  ngOnInit() {
    this._hubConnection = new HubConnectionBuilder().withUrl(API + '/chat', {
      accessTokenFactory: () => {
        return window.localStorage.getItem('access_token');
      }
     },
    ).build();
    this._hubConnection
      .start().then(() => {
        this.id.subscribe((id) => {
          this._hubConnection.invoke('SwitchGroup', id).catch(this.Handler);
          this._hubConnection.invoke('SendAllMessages', id).catch(this.Handler);
        });
      })
      .catch(err => console.log('Error while establishing connection :('));
    this._hubConnection.on('reciveAllMessages', (msgs: MessageModel[]) => {
      this.messages = msgs;
      const chatElement = document.querySelector('.chat');
      setTimeout(() => {
       chatElement.scrollTop = chatElement.scrollHeight;
      }, 0);
    });
    this._hubConnection.on('reciveMessage', (msg: MessageModel) => {
         this.messages.push(msg);
         const chatElement = document.querySelector('.chat');
         setTimeout(() => {
          chatElement.scrollTop = chatElement.scrollHeight;
         }, 0);
    });
  }
  SetID(id: string) {
    this.id.next(id);
  }
  Send() {
    // this.id.subscribe((id) => {
    //   this._hubConnection.invoke('SendAllMessages', id);
    // });
    this._hubConnection.invoke('sendMessage', this.message, this.id.value ).catch(this.Handler);
    this.message = '';
  }
  Handler(err) {
      const error = err.message.toString();
      if (error.indexOf('because user is unauthorized') !== -1) {
        this.notify.create(
          'error',
          'Error',
          'Token expired! We will redirect you to the login page.',
        );
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
     } else {
      this.notify.create(
        'error',
        'Error',
        error,
      );
     }
    }
}
