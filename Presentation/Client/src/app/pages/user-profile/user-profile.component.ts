import { Component } from '@angular/core';
import { UserProfileService } from './services/user-profile.service';
import { ActivatedRoute } from '@angular/router';
import { UserProfile } from './interfaces/user-profile';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.scss'
})
export class UserProfileComponent {
  user?: UserProfile;

  constructor(private userService: UserProfileService, private activatedRoute: ActivatedRoute) {}

  ngOnInit(){
    this.loadUser();
  }

  loadUser(){
    const userId = this.activatedRoute.snapshot.paramMap.get('id');
    if(userId){
      this.userService.getUser(userId).subscribe({
        next: (response) => this.user = response,
        error: (error) => console.log(error.error)
      });
    }
  }
}
