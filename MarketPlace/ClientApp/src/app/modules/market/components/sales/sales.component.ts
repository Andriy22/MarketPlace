import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator } from '@angular/material';
import { Order } from 'src/app/shared/models/Order';
import { MarketService } from 'src/app/shared/services/market.service';

@Component({
  selector: 'app-sales',
  templateUrl: './sales.component.html',
  styleUrls: ['./sales.component.css']
})
export class SalesComponent implements OnInit {
  displayedColumns: string[] = ['Id', 'Description', 'Buyer', 'Status', 'Price'];
  dataSource: MatTableDataSource<Order>;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  constructor(private mS: MarketService) { }

  ngOnInit() {
    this.mS.getSales().subscribe((x: Order[]) => {
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
