import { Router } from '@angular/router';
import { NzNotificationService } from 'ng-zorro-antd';
export class  ErrorHandler {

 constructor(private router: Router, private  notify: NzNotificationService) {}
 public  ErrorHandle(err) {
    const error = err.message.toString();
    console.log(error);
    if (error.indexOf('because user is unauthorized') !== -1) {
      this.notify.create(
        'error',
        'Error',
        err,
      );
   }

 }
}
