import { Component } from '@angular/core';
import { LogoutService } from './services/logout.service';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-logout',
  standalone: true,
  imports: [
    RouterModule
  ],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.scss'
})
export class LogoutComponent {
  constructor(private logoutService: LogoutService, private authService: AuthenticationService, private router: Router) {}

  logout() {
    this.logoutService.logout().subscribe({
      next: () => {
        this.authService.clearToken();
        console.log("Logged out successfully.");
        this.router.navigate(['login']);
      },
      error: () => {
        console.log("Error logging out.");
      }
    })
  }
}
