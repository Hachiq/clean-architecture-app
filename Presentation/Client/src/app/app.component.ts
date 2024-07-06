import { Component } from '@angular/core';
import { AuthenticationService } from './shared/services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Client';

  constructor(private authService: AuthenticationService) {
    authService.refreshToken().subscribe({
      next: (accessToken) => {
        console.log("Access token refreshed");
        authService.setToken(accessToken)
      },
      error: (e) => {
        console.log(`Error during token refresh: ${e.error}`);
      }
    });
  }
}
