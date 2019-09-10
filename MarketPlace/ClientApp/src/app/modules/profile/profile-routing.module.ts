import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { ChatforuserComponent } from './components/chatforuser/chatforuser.component';
import { AuthGuard } from 'src/app/shared/guards/auth.guard';
import { ChangePasswordComponent } from './components/change-password/change-password.component';


const routes: Routes = [
  {path: '', component: HomeComponent, canActivate: [AuthGuard]},
  {path: 'messages', component: ChatforuserComponent, canActivate: [AuthGuard]},
  {path: 'password', component: ChangePasswordComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProfileRoutingModule { }
