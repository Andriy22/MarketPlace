import { Component, OnInit } from '@angular/core';
import {AuthenticationService} from './shared/services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'ClientApp';
  isAuth = false;
  constructor(protected  AuthService: AuthenticationService) {
  }
  ngOnInit() {
    this.AuthService.isAuth.subscribe((x) => {
      setTimeout(() => {
        this.isAuth = x;
      }, 0);
    });
  }
}
