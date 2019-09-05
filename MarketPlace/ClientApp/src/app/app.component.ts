import { Component, OnInit } from '@angular/core';
import {AuthenticationService} from './shared/services/authentication.service';
import { ProfileService } from './shared/services/profile.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'ClientApp';
  isAuth = false;
  balance = 0;
  constructor(protected  AuthService: AuthenticationService, private pS: ProfileService) {
  }
  ngOnInit() {
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
