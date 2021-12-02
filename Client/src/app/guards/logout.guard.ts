import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class LogoutGuard implements CanActivate {
  constructor(private accountService:AccountService, private router:Router){}
  
  canActivate(): boolean  {
    let isAuthorized = this.accountService && this.accountService.GetCurrentUser() != null;
    if(isAuthorized){
      this.router.navigateByUrl('weather');
      return false;
    }
    return true;
  }
  
}
