import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  forecast: any;
  constructor(private http: HttpClient) {}

  ngOnInit(){
    this.http.get(`${environment.apiUrl}/weatherforecast`).subscribe({
      next: (response) => {
        this.forecast = response;
      },
      error: (e) => {
        console.log(`Error during token refresh: ${e.error}`);
      }
    })
  }
}
