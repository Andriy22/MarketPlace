import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Lot } from 'src/app/shared/models/lot';
import { MarketService } from 'src/app/shared/services/market.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatTableDataSource, MatSort } from '@angular/material';
import {MatPaginator} from '@angular/material/paginator';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {
  dataSource: MatTableDataSource<Lot>;
  displayedColumns: string[] = ['Name', 'Price'];
  id: string = null;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  constructor(private Ms: MarketService, private spinner: NgxSpinnerService, private route: ActivatedRoute) { }


  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    this.spinner.show();
    // tslint:disable-next-line: radix
    this.Ms.GetLots(Number.parseInt(this.id)).subscribe((data: Lot[]) => {
      this.dataSource = new MatTableDataSource(data);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.spinner.hide();
   }, (err) => {
      this.spinner.hide();
      console.warn(err);
   });
  }
  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}


