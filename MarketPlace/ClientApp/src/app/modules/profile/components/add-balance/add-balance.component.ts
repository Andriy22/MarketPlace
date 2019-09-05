import { Component, OnInit } from '@angular/core';
import { ProfileService } from 'src/app/shared/services/profile.service';
import { NzNotificationService } from 'ng-zorro-antd';

@Component({
  selector: 'app-add-balance',
  templateUrl: './add-balance.component.html',
  styleUrls: ['./add-balance.component.css']
})
export class AddBalanceComponent implements OnInit {

  isVisible = false;
  isOkLoading = false;
  code: string = null;


  constructor(private pS: ProfileService,   private  notify: NzNotificationService) { }

  ngOnInit() {
  }

  showModal(): void {
    this.isVisible = true;
  }

  handleOk(): void {
    this.isOkLoading = true;
    this.pS.addBalance(this.code).subscribe((data) => {
      this.isOkLoading = false;
      this.isVisible = false;
      this.notify.create(
        'success',
        'Success',
        'Added ' + data + ' UAN!',
      );
    }, (err) => {
      this.isOkLoading = false;
      this.isVisible = false;
      this.notify.create(
        'error',
        'Error',
        err,
      );
    });


  }

  handleCancel(): void {
    this.isVisible = false;
  }
}
