import { Component } from '@angular/core';
import { UserProfileService } from './services/user-profile.service';
import { ActivatedRoute } from '@angular/router';
import { UserProfile } from './interfaces/user-profile';
import { environment } from 'src/environments/environment';
import { MatExpansionModule } from '@angular/material/expansion';
import { NgIf } from '@angular/common';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    NgIf,
    MatExpansionModule,
    MatInputModule
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss'
})
export class UserProfileComponent {
  user?: UserProfile;
  pictureUrl?: string;

  constructor(private userService: UserProfileService, private activatedRoute: ActivatedRoute) {}

  ngOnInit(){
    this.loadUser();
  }

  loadUser(){
    const userId = this.activatedRoute.snapshot.paramMap.get('id');
    if(userId){
      this.userService.getUser(userId).subscribe({
        next: (response) => {
          this.user = response
          this.pictureUrl = `${environment.apiUrl}/${response.profilePictureUrl}`
        },
        error: (error) => console.log(error.error)
      });
    }
  }
}
