import { Component, OnInit } from '@angular/core';
import { MarketService } from 'src/app/shared/services/market.service';
import { LotModel } from 'src/app/shared/models/lotModel';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-lot',
  templateUrl: './lot.component.html',
  styleUrls: ['./lot.component.css']
})
export class LotComponent implements OnInit {
  data: LotModel;
  constructor(private Ms: MarketService, private spinner: NgxSpinnerService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.spinner.show();
    const id = this.route.snapshot.paramMap.get('id');
    this.Ms.getLot(id).subscribe((x: LotModel) => {
      this.spinner.hide();
      this.data = x;
      console.log(x);
    }, (err) => {
      this.spinner.hide();
      console.warn(err);
    });
  }
}
