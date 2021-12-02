import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {

  constructor(private accountService:AccountService, private router:Router){}
  canActivate(): boolean {
    let bAuthorized = this.accountService && this.accountService.GetCurrentUser() != null;
    if(!bAuthorized)
    {
      this.router.navigateByUrl('account');
      return false;
    }
    return true;
  }
  
}
