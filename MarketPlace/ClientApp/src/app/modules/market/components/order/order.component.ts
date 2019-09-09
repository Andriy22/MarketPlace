import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { NzMessageService, NzNotificationService } from 'ng-zorro-antd';
import { MarketService } from 'src/app/shared/services/market.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ActivatedRoute, Router } from '@angular/router';
import { Order } from 'src/app/shared/models/Order';
import { ChatforlotComponent } from '../lots/chatforlot/chatforlot.component';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  data: Order;
  @ViewChild(ChatforlotComponent, {static: true})
  private Chat: ChatforlotComponent;
  constructor(private nzMessageService: NzMessageService, private Ms: MarketService, private spinner: NgxSpinnerService,
              private route: ActivatedRoute, private notification: NzNotificationService, private router: Router,
              protected aS: AuthenticationService) { }

              ngOnInit() {
                this.spinner.show();
                const id = this.route.snapshot.paramMap.get('id');
                this.Ms.getOrder(id).subscribe((x: Order) => {
                  this.spinner.hide();
                  this.data = x;
                  this.Chat.SetUserName(x.saller);
                  console.log(x);
                }, (err) => {
                  this.spinner.hide();
                  console.warn(err);
                });
              }
              cancel(): void {
                this.nzMessageService.info('click cancel');
              }

              confirm(element): void {
                this.Ms.Confirm(this.data.id);
              }
              ReturnMon(element): void {
                this.Ms.ReturnMoney(this.data.id);
                this.notification.remove('ret');
              }
              createBasicNotification(template: TemplateRef<{}>): void {
                this.notification.template(template);
              }

}
