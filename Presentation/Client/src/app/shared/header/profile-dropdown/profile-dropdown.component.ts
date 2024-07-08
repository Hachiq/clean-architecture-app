import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatMenuModule } from '@angular/material/menu';
import { LogoutComponent } from '../../logout/logout.component';
import { AuthenticationService } from '../../services/authentication.service';
import { CommonModule, NgIf } from '@angular/common';

@Component({
  selector: 'app-profile-dropdown',
  standalone: true,
  imports: [
    RouterModule,
    MatMenuModule,
    LogoutComponent,
    NgIf,
    CommonModule
  ],
  templateUrl: './profile-dropdown.component.html',
  styleUrl: './profile-dropdown.component.scss'
})
export class ProfileDropdownComponent {
  constructor(public authService: AuthenticationService) {}
}
