import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator } from '@angular/material';
import { Order } from 'src/app/shared/models/Order';
import { MarketService } from 'src/app/shared/services/market.service';

@Component({
  selector: 'app-purchases',
  templateUrl: './purchases.component.html',
  styleUrls: ['./purchases.component.css']
})
export class PurchasesComponent implements OnInit {

  displayedColumns: string[] = ['Id', 'Description', 'Buyer', 'Status', 'Price'];
  dataSource: MatTableDataSource<Order>;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  constructor(private mS: MarketService) { }

  ngOnInit() {
    this.mS.getPurchases().subscribe((x: Order[]) => {
      console.log(x);
      this.dataSource = new MatTableDataSource(x);
      this.dataSource.paginator = this.paginator;
    });

  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}
