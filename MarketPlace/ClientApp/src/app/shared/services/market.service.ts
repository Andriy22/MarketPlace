import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Game } from '../models/games';
import { HttpClient } from '@angular/common/http';
import { API } from './../config';
import { NzNotificationService } from 'ng-zorro-antd';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class MarketService {
  public Games: BehaviorSubject<Game[]>;
  constructor(private http: HttpClient,
              private  notify: NzNotificationService,
              private  spinner: NgxSpinnerService,
              private router: Router) {
    this.Games = new BehaviorSubject<Game[]>(null);
    this.getGames();
  }
  getGames() {
    this.spinner.show();
    this.http.get(API + '/api/market/getgames').subscribe((x: Game[]) => {
      this.spinner.hide();
      // console.log('service', x );
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

  GetLots(id: number) {
    return this.http.get(API + '/api/market/getLots?id=' + id);
  }
  GetMyLots(id: number) {
    return this.http.get(API + '/api/market/getMyLots?id=' + id);
  }

  newLot(Category: string, Name: string, Description: string, Price: string) {
    this.spinner.show();
    this.http.post(API + '/api/market/newLot', {Category, Name, Description, Price}).subscribe(() => {
      this.spinner.hide();
      this.router.navigate(['/category/' + Category]);
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
