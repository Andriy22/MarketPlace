import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Game } from '../models/games';
import { HttpClient } from '@angular/common/http';
import { API } from './../config';
import { NzNotificationService } from 'ng-zorro-antd';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class MarketService {
public Games: BehaviorSubject<Game[]>;
constructor(private http: HttpClient,
            private  notify: NzNotificationService,
            private  spinner: NgxSpinnerService) {
  this.Games = new BehaviorSubject<Game[]>(null);
  this.getGames();
}
getGames() {
  this.spinner.show();
  this.http.get(API + '/api/market/getgames').subscribe((x: Game[]) => {
    this.spinner.hide();
    console.log('service', x );
    this.Games.next(x);
  }, (err) => {
    this.spinner.hide();
    this.notify.create(
      'error',
      'Error',
      err,
    );
  });
}
}
