import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/app/services/http.service';

@Component({
  selector: 'app-weather-details',
  templateUrl: './weather-details.component.html',
  styleUrls: ['./weather-details.component.css']
})
export class WeatherDetailsComponent implements OnInit {

  weatherList:any[] = [];
  constructor( private httpService: HttpService) { }

  ngOnInit(): void {
    this.GetWeatherFromServer();
  }

  GetWeatherFromServer():void{
    this.httpService.GetWeathers().subscribe(
      (res:any) => {
        this.weatherList = res;
      },err=>{
        console.log(err);      
      }
    )
  }
}
