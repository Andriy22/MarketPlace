import { Component, OnInit } from '@angular/core';
import {AuthenticationService} from './shared/services/authentication.service';
import { ProfileService } from './shared/services/profile.service';
import { Router, RouteConfigLoadStart, RouteConfigLoadEnd } from '@angular/router';
import { Spinner } from 'ngx-spinner/lib/ngx-spinner.enum';
import { NgxSpinnerService } from 'ngx-spinner';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { API } from './shared/config';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'ClientApp';
  isAuth = false;
  balance = 0;
  sales = 0;
  pursh = 0;
  private _hubConnection: HubConnection;
  constructor(protected  AuthService: AuthenticationService, private pS: ProfileService, private router: Router,
              private spinner: NgxSpinnerService) {
  }
  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof RouteConfigLoadStart) {
         this.spinner.show();
      } else if (event instanceof RouteConfigLoadEnd) {
         this.spinner.hide();
      }
  });

    this.AuthService.isAuth.subscribe((x) => {
      if (x) {
        this._hubConnection = new HubConnectionBuilder().withUrl(API + '/chat', {
          accessTokenFactory: () => {
            return window.localStorage.getItem('access_token');
          }
         },
        ).build();
        this._hubConnection
          .start().then(() => {
              this._hubConnection.invoke('getPursh');
              this._hubConnection.invoke('getSales');
          })
          .catch(err => console.log('Error while establishing connection :('));


        this._hubConnection.on('getSales', (sales: number) => {
            setTimeout(() => {
              this.sales = sales;
            }, 0);
          });
        this._hubConnection.on('getPurchases', (pursh: number) => {
            setTimeout(() => {
              this.pursh = pursh;
            }, 0);
          });
      }
      setTimeout(() => {
        this.isAuth = x;
      }, 0);
    });
    this.pS.getMyBalance.subscribe((x) => {
      this.balance =  x;
    });
  }
}
