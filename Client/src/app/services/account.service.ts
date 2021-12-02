import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  loginUrl = './api/Account/Login';
  logoutUrl = './api/Account/Logout';
  redirectUrl = '/weather/weather-details'
  constructor(private http:HttpClient, private router:Router) { }
  Login(UserInf:any):void{
    this.http.post(this.loginUrl, UserInf).subscribe(
      () => {
      this.router.navigateByUrl(this.redirectUrl, {replaceUrl:true})
    }, error => {
      console.log(error);
    });
  }

  Logout():void{
    this.http.get(this.logoutUrl).subscribe(
      () =>{
        this.router.navigateByUrl('/account/login');
      }
    );
  }

  GetCurrentUser():any{
    let arr;
    if(arr = document.cookie.match(new RegExp("(^| )UserName=([^;]*)(;|$)")))
    {
      return unescape(arr[2]);
    }
    return null;
  }
}
