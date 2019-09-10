import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { MatSnackBar } from '@angular/material';
import { NzNotificationService } from 'ng-zorro-antd';
import { ProfileService } from 'src/app/shared/services/profile.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  Form: FormGroup;
  error: string;
  constructor(private formBuilder: FormBuilder,
              private Spinner: NgxSpinnerService,
              private pS: ProfileService,
              private notify: NzNotificationService) {
  }

  ngOnInit() {
    this.Form = this.formBuilder.group({
      currentPassword: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]]
    },
      {validator: this.checkPasswords });
  }
  onSubmit() {
    const CurrentPass  = this.f.currentPassword.value;
    const NewPassword = this.f.confirmPassword.value;
    this.Spinner.show();
    this.pS.changePassword(CurrentPass, NewPassword).subscribe((x) => {
      this.Spinner.hide();
      this.notify.create(
        'success',
        'OK',
        'Your password changed!'
      );
    }, (x) => {
      this.Spinner.hide();
      // this.error = x;
      this.notify.create(
        'error',
        'Error',
        x
      );
    });
    this.Form.reset();
  }
  get f() {
    return this.Form.controls;
  }
  checkPasswords(group: FormGroup) {
    const pass = group.controls.password.value;
    const confirmPass = group.controls.confirmPassword.value;
    if (pass === confirmPass) {
      return null;
    } else {
      group.controls.confirmPassword.setErrors({MatchPassword: true});
    }
  }

}
