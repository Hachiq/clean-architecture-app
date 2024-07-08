import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { CommonModule, NgIf } from '@angular/common';
import { LogoutComponent } from '../logout/logout.component';
import { LoadingService } from '../services/loading.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatToolbarModule,
    RouterModule,
    NgIf,
    LogoutComponent,
    CommonModule
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  constructor(public authService: AuthenticationService, public loadingService: LoadingService) {}

  authorized(): boolean {
    return this.authService.isAuthorized();
  }
}
