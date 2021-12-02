import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import {tap} from 'rxjs/operators';
import { AccountService } from '../services/account.service';

@Injectable()
export class HttpsInterceptor implements HttpInterceptor {

  constructor(private accountService:AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let headers = request.headers;
    headers = headers.set("Content-Type", "application/json");
    return next.handle(request).pipe(
      tap(
        event => {
            // trace
        },  error =>{
          if(error.status === 401){
            console.log("User session end already!");
            this.accountService.Logout();
          }
        }
      )
    );  
  }
}
