import { Component, OnInit, ViewChild, DoCheck } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { Game } from './../../../../shared/models/games';
import { MarketService } from './../../../../shared/services/market.service';
import { async } from 'q';
import { ChatComponent } from '../chat/chat.component';



@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {


  displayedColumns: string[] = ['game', 'categories'];
  dataSource: MatTableDataSource<Game>;
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(ChatComponent, {static: true})
  private Chat: ChatComponent;
  constructor(private Ms: MarketService) {
    this.Ms.Games.subscribe((x) => {
     // console.log(x);
      this.dataSource = new MatTableDataSource(x);
    });
    // console.log(this.Games);

  }
  // ngDoCheck(): void {
  //   this.Chat.SetID('0');
  // }
  ngOnInit() {

    this.dataSource.paginator = this.paginator;
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}

// /** Builds and returns a new User. */
// function createNewUser(id: number): UserData {
//   const name = NAMES[Math.round(Math.random() * (NAMES.length - 1))] + ' ' +
//       NAMES[Math.round(Math.random() * (NAMES.length - 1))].charAt(0) + '.';

//   return {
//     id: id.toString(),
//     name: name,
//     progress: Math.round(Math.random() * 100).toString(),
//     color: COLORS[Math.round(Math.random() * (COLORS.length - 1))]
//   };
// }


