import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API } from './../config';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
private myBalance: BehaviorSubject<number>;
constructor(private http: HttpClient, private aS: AuthenticationService) {
  this.myBalance = new  BehaviorSubject<number>(0);
  // this.getBalance();
  // this.aS.isAuth.subscribe((x) => {
  //   if ( x ) {
  //     this.getBalance();
  //   }
  // });
}
public get getMyBalance() {

  return this.myBalance;
}
addBalance(code: string) {
  return this.http.get(API + '/api/profile/addbalance?code=' + code);
}

// private getBalance() {
//   this.http.get(API + '/api/profile/myBalance').subscribe((x: number) => {
//     this.myBalance.next(x);
//   });
// }

}
