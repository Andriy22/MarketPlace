import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DefaultComponent } from './default.component';
import {NgZorroAntdModule} from 'ng-zorro-antd';
import {RouterModule} from '@angular/router';
import { RegisterComponent } from './components/register/register.component';

@NgModule({
  declarations: [
    RegisterComponent,
    DefaultComponent
  ],
  imports: [
    RouterModule.forChild([
      {path: '', component: DefaultComponent},
      {path: 'register', component: RegisterComponent}
    ]),
    CommonModule,
    NgZorroAntdModule,
  ],
  exports: [
    RouterModule,
  ],
  bootstrap: [DefaultComponent]
})
export class DefaultModule { }
