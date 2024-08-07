import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { WeatherForecasts } from "@app/models/weather-forecast.model";

@Component({
  selector: 'f-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'Financist';
  forecasts: WeatherForecasts = [];

  constructor(http: HttpClient) {
    http.get<WeatherForecasts>('api/WeatherForecast').subscribe({
      next: result => this.forecasts = result,
      error: console.error
    });
  }
}
