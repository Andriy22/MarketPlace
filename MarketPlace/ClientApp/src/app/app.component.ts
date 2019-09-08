import { Component, OnInit } from '@angular/core';
import {AuthenticationService} from './shared/services/authentication.service';
import { ProfileService } from './shared/services/profile.service';
import { Router, RouteConfigLoadStart, RouteConfigLoadEnd } from '@angular/router';
import { Spinner } from 'ngx-spinner/lib/ngx-spinner.enum';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'ClientApp';
  isAuth = false;
  balance = 0;
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
      setTimeout(() => {
        this.isAuth = x;
      }, 0);
    });
    this.pS.getMyBalance.subscribe((x) => {
      this.balance =  x;
    });
  }
}
