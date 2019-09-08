import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {NzNotificationService} from 'ng-zorro-antd';
import {RegisterService} from '../../../shared/services/register.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  visible = false;

  open(): void {
    this.visible = true;
  }

  RegisterForm: FormGroup;
  error: string;

  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private route: ActivatedRoute,
              private  spinner: NgxSpinnerService,
              private  notify: NzNotificationService,
              private  RegService: RegisterService) {
  }

  ngOnInit() {
    this.RegisterForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required]],
      password: ['', [Validators.required]],
      confirmPassword: ['', Validators.required]
    },
      { validator: this.checkPasswords }
    );
  }

  onSubmit() {
    const data = this.RegisterForm.value;
    this.spinner.show();

    this.RegService.register(data.email, data.username, data.password).subscribe(() => {
      this.spinner.hide();
        this.router.navigate(['/login']);
    }, (error) => {
      this.spinner.hide();
      this.notify.create(
        'error',
        'Error',
        error,
      );
    });
  }
  checkPasswords(group: FormGroup) {
    const pass = group.controls.password.value;
    const confirmPass = group.controls.confirmPassword.value;

    if (pass === confirmPass) {
      return null;
    } else {
      group.controls.confirmPassword.setErrors({ MatchPassword: true });
    }
  }

}
