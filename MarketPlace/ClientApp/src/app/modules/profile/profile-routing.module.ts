import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ChatforuserComponent } from './components/chatforuser/chatforuser.component';


const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'messages', component: ChatforuserComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProfileRoutingModule { }
