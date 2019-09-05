import { Component, OnInit, ViewChild } from '@angular/core';
import { AddBalanceComponent } from '../add-balance/add-balance.component';
import { ProfileService } from 'src/app/shared/services/profile.service';

@Component({
  selector: 'app-profile-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  balance = 0;
  @ViewChild(AddBalanceComponent, {static: false})
  private AddBalance: AddBalanceComponent;
  constructor(private pS: ProfileService) { }

  ngOnInit() {
    this.pS.getMyBalance.subscribe((x) => {
      this.balance =  x;
    });
  }
  Show() {
    this.AddBalance.showModal();
  }
}
