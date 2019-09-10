import { Component, OnInit, ViewChild } from '@angular/core';
import { AddBalanceComponent } from '../add-balance/add-balance.component';
import { ProfileService } from 'src/app/shared/services/profile.service';
import { HubConnection, HubConnectionBuilder, HttpTransportType, LogLevel } from '@aspnet/signalr';
import { API } from './../../../../shared/config';
@Component({
  selector: 'app-profile-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  balance = 0;
  // tslint:disable-next-line: variable-name
  private _hubConnection: HubConnection;
  @ViewChild(AddBalanceComponent, {static: false})
  private AddBalance: AddBalanceComponent;
  constructor(private pS: ProfileService) { }

  ngOnInit() {

    this._hubConnection = new HubConnectionBuilder().withUrl(API + '/chat', {
      accessTokenFactory: () => {
        return window.localStorage.getItem('access_token');
      }
     },
    ).build();
    this._hubConnection
      .start()
      .then(() => {
        console.log('Connection started!');
        this._hubConnection.invoke('Send', '123').catch((reason) => {
          console.log(reason);
        });
      })
      .catch(err => console.log('Error while establishing connection :('));

    this._hubConnection.on('sendBalance', (balance: number) => {
      this.balance = balance;
    });

    // this.pS.getMyBalance.subscribe((x) => {
    //   this.balance =  x;
    // });
  }
  Show() {
    this.AddBalance.showModal();
  }
}
