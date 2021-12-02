import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  UserInfo:any = {};
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  Login():void{
    console.log(this.UserInfo);
    this.accountService.Login(this.UserInfo);
  }
}
