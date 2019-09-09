import { Component, OnInit, ViewChild } from '@angular/core';
import { MarketService } from 'src/app/shared/services/market.service';
import { LotModel } from 'src/app/shared/models/lotModel';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { ChatforlotComponent } from '../chatforlot/chatforlot.component';
import { NzNotificationService } from 'ng-zorro-antd';

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
              private notify: NzNotificationService, private router: Router) { }

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
  Buy() {
    this.spinner.show();
    this.Ms.Buy(this.data.id).subscribe((x) => {
      this.spinner.hide();
      this.router.navigate(['/purchases']);
    }, (err) => {
      this.spinner.show();
      this.notify.create(
        'error',
        'Error!',
        err,
      );
    });
  }
}
