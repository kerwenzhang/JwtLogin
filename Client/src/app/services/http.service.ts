import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  baseUrl = './api/';  
  constructor(private http:HttpClient, private router:Router) { }

  GetWeathers():Observable<any>{
    return this.http.get(this.baseUrl + 'weatherforecast');
  }

  
}
