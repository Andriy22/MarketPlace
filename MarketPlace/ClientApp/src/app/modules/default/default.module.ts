import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {NgZorroAntdModule} from 'ng-zorro-antd';
import {RouterModule} from '@angular/router';
import { RegisterComponent } from './components/register/register.component';

@NgModule({
  declarations: [
    RegisterComponent,
    
  ],
  imports: [
    RouterModule.forChild([
     
      {path: 'register', component: RegisterComponent}
    ]),
    CommonModule,
    NgZorroAntdModule,
  ],
  exports: [
    RouterModule,
  ],

})
export class DefaultModule { }
