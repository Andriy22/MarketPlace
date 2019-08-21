import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutcomponentComponent } from './layoutcomponent/layoutcomponent.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzSliderModule } from 'ng-zorro-antd/slider';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzGridModule } from 'ng-zorro-antd/grid';


@NgModule({
  declarations: [LayoutcomponentComponent],
  imports: [
    CommonModule,
    NzButtonModule,
    NzLayoutModule,
    NzGridModule,
    NzDropDownModule,
    NzBreadCrumbModule,
    NzAvatarModule,
    NzMenuModule,
    NzSliderModule
  ],
  exports: [LayoutcomponentComponent]
})
export class UserLayoutModule { }
