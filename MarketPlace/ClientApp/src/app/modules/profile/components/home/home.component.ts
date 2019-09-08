import { Component, OnInit, ViewChild } from '@angular/core';
import { AddBalanceComponent } from '../add-balance/add-balance.component';
import { ProfileService } from 'src/app/shared/services/profile.service';
import { HubConnection, HubConnectionBuilder, HttpTransportType, LogLevel } from '@aspnet/signalr';
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

    this._hubConnection = new HubConnectionBuilder().withUrl('https://localhost:44333/chat', {
      accessTokenFactory: () => {
        return window.localStorage.getItem('access_token');
      }
     },
    ).build();
    this._hubConnection
      .start()
      .then(() => {
        console.log('Connection started!');
        this._hubConnection.invoke('Send', '123');
      })
      .catch(err => console.log('Error while establishing connection :('));
    console.log(this._hubConnection.state);

    this._hubConnection.on('sendBalance', (balance: number) => {
      console.log(balance);
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
