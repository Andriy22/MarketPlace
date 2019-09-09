import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { CategoryComponent } from './components/category/category.component';
import { AddLotComponent } from './components/lots/add-lot/add-lot.component';
import { MyLotsComponent } from './components/lots/my-lots/my-lots.component';
import { LotComponent } from './components/lots/lot/lot.component';
import { SalesComponent } from './components/sales/sales.component';
import { OrderComponent } from './components/order/order.component';
import { PurchasesComponent } from './components/purchases/purchases.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'category/:id', component: CategoryComponent},
  {path: 'lot/add/:id', component: AddLotComponent},
  {path: 'lot/my-lots/:id', component: MyLotsComponent},
  {path: 'lot/:id', component: LotComponent},
  {path: 'sales', component: SalesComponent},
  {path: 'purchases', component: PurchasesComponent},
  {path: 'order/:id', component: OrderComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
exports: [RouterModule]
})
export class MarketRoutingModule { }
