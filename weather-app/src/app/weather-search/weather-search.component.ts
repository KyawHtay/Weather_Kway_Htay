import { Component } from '@angular/core';
import { WeatherService } from '../services/weather.service';
import { Weather } from '../models/weather';

@Component({
  selector: 'app-weather-search',
  templateUrl: './weather-search.component.html',
  styleUrl: './weather-search.component.css'
})
export class WeatherSearchComponent {
  city: string = '';
  countryCode: string = 'GB';
  weatherData: Weather | undefined;

  errorMessage: string = '';

  constructor(private weatherService: WeatherService) {}
  searchWeather() {
    if (!this.city) {
      this.errorMessage = 'Please enter a city';
      return;
    }
    this.weatherService.getWeather(this.city, this.countryCode).subscribe(
      (data) => {
        this.weatherData = data;
        this.errorMessage = '';
        console.log(  this.weatherData );
      },
      (error) => {
        this.errorMessage = 'Failed to fetch weather data';
      }
    );
  }
}
