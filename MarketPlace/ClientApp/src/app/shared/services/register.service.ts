import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {API} from '../config';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {

  constructor(private  http: HttpClient) { }

  register(email, username, password) {
    return this.http.post(API + '/api/register/register', {email, nickname: username, password});
  }
}
