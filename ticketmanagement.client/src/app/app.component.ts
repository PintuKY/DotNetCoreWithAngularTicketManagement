import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TicketService, Ticketlist } from './services/ticket.service';
interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public forecasts: WeatherForecast[] = [];
  public resulttickets: Ticketlist[] = [];
  //constructor(private http: HttpClient) { }
  constructor(private ticketService: TicketService) { }

  ngOnInit() {
    this.getForecasts();
  }

  //getForecasts() {
  //  this.http.get<WeatherForecast[]>('/weatherforecast').subscribe(
  //    (result) => {
  //      this.forecasts = result;
  //    },
  //    (error) => {
  //      console.error(error);
  //    }
  //  );
  //}
  getForecasts() {
    this.ticketService.FetchTicketsForecast().subscribe(
      (result) => {
        this.resulttickets = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  title = 'ticketmanagement.client';
}
