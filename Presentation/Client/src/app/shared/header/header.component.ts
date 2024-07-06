import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { NgIf } from '@angular/common';
import { LogoutComponent } from '../logout/logout.component';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatToolbarModule,
    RouterModule,
    NgIf,
    LogoutComponent
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  constructor(private authService: AuthenticationService) {}

  authorized(): boolean {
    return this.authService.isAuthorized();
  }
}
