import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Weather } from '../models/weather';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  private apiUrl = 'http://localhost:5125/api/weather';
  constructor(private http: HttpClient) {}
  getWeather(city: string, countryCode: string): Observable<Weather> {
    return this.http.get<Weather>(`${this.apiUrl}/${city}?countrycode=${countryCode}`);
  }
}
