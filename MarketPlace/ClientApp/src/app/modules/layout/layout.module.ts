import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NarbarComponent } from './components/narbar/narbar.component';
import { NavbarComponent } from './components/navbar/navbar.component';



@NgModule({
  declarations: [NarbarComponent, NavbarComponent],
  imports: [
    CommonModule
  ]
})
export class LayoutModule { }
