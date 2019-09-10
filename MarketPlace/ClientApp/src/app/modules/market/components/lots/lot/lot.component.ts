import { Component, OnInit, ViewChild } from '@angular/core';
import { MarketService } from 'src/app/shared/services/market.service';
import { LotModel } from 'src/app/shared/models/lotModel';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatforlotComponent } from '../chatforlot/chatforlot.component';
import { NzNotificationService } from 'ng-zorro-antd';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { MatSlideToggle } from '@angular/material';

@Component({
  selector: 'app-lot',
  templateUrl: './lot.component.html',
  styleUrls: ['./lot.component.css']
})
export class LotComponent implements OnInit {
  data: LotModel;
  @ViewChild(ChatforlotComponent, {static: true})
  private Chat: ChatforlotComponent;
  constructor(private Ms: MarketService, private spinner: NgxSpinnerService, private route: ActivatedRoute,
              private notify: NzNotificationService, private router: Router,  public aS: AuthenticationService) { }

  ngOnInit() {
    this.spinner.show();
    const id = this.route.snapshot.paramMap.get('id');
    this.Ms.getLot(id).subscribe((x: LotModel) => {
      this.spinner.hide();
      this.data = x;
      this.Chat.SetUserName(x.userName);
      console.log(x);
    }, (err) => {
      this.spinner.hide();
      console.warn(err);
    });
  }
  changeStatus(event: MatSlideToggle) {
    this.spinner.show();
    this.Ms.changeStatus(this.data.id, event.checked.toString()).subscribe((x) => {
      this.spinner.hide();
      this.notify.create(
        'success',
        'Success',
        'Status updated!',
      );
    }, (err) => {
      this.spinner.hide();
      this.notify.create(
        'error',
        'Error!',
        err,
      );
    });

  }
  Delete() {
    this.spinner.show();
    this.Ms.deleteLot(this.data.id).subscribe((x) => {
      this.spinner.hide();
      this.notify.create(
        'success',
        'Success',
        'Lot deleted!',
      );
      this.router.navigate(['/']);
    }, (err) => {
      this.spinner.hide();
      this.notify.create(
        'error',
        'Error!',
        err,
      );
    });
  }
  Buy() {
    this.spinner.show();
    this.Ms.Buy(this.data.id).subscribe((x) => {
      this.spinner.hide();
      this.router.navigate(['/purchases']);
    }, (err) => {
      this.spinner.hide();
      this.notify.create(
        'error',
        'Error!',
        err,
      );
    });
  }
}
