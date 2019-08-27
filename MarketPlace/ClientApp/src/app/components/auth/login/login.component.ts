import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {NzNotificationService} from 'ng-zorro-antd';
import {AuthenticationService} from '../../../shared/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  error: string;

  constructor(private formBuilder: FormBuilder,
              private router: Router,
              private route: ActivatedRoute,
              private  spinner: NgxSpinnerService,
              private  notify: NzNotificationService,
              private  AuthService: AuthenticationService) {
  }

  ngOnInit() {
    this.AuthService.logout();
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]]
    });
  }

  onSubmit() {
    const data = this.loginForm.value;
    console.log(data.email);
    this.spinner.show();



    this.AuthService.login(data.email, data.password).subscribe(() => {
      this.spinner.hide();
      if (this.route.snapshot.queryParamMap.get('returnUrl')) {
        this.router.navigate([this.route.snapshot.queryParamMap.get('returnUrl')]);
      } else {
        this.router.navigate(['/']);
      }
    }, (error) => {
      this.spinner.hide();
      this.notify.create(
        'error',
        'Error',
        error,
      );
    });
  }
}
