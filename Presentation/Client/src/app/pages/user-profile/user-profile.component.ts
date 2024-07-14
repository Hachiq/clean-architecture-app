import { Component } from '@angular/core';
import { UserProfileService } from './services/user-profile.service';
import { ActivatedRoute } from '@angular/router';
import { UserProfile } from './interfaces/user-profile';
import { environment } from 'src/environments/environment';
import { MatExpansionModule } from '@angular/material/expansion';
import { NgIf } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    NgIf,
    MatExpansionModule,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule
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

  firstName = new FormControl('');
  lastName = new FormControl('');
  updateContacts(id: string){
    this.userService.updateContacts(id, {
      firstName: this.firstName.value,
      lastName: this.lastName.value
    }).subscribe({
      next: () => {
        this.loadUser();
        this.firstName.reset();
        this.lastName.reset();
      }
    });
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
