import { Component } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button'
import { FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RegisterService } from './services/register.service';
import { Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    HttpClientModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  constructor(private registerService: RegisterService, private router: Router) { }

  hide = true;

  username = new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]);
  email = new FormControl('', [Validators.required, Validators.email]);
  password = new FormControl('', [Validators.required, Validators.minLength(6)]);

  register() {
    this.registerService.register({
      username: this.username.value,
      email: this.email.value,
      password: this.password.value
    }).subscribe({
      next: () => {
        console.log(`User ${this.username.value} was registered successfully`);
        this.router.navigate(['login']);
      },
      error: (e) => {
        if(e.status === 409){
          const reason = e.error.reason;
          
          if (reason === "UsernameTaken") {
            this.username.setErrors({ conflict: true });
          }
          else if (reason === "EmailTaken") {
            this.email.setErrors({ conflict: true });
          }
        }
        else {
          console.log("Unknown error.");
        }
      }
    })
  }

  getUsernameErrorMessage() {
    if (this.username.hasError('required')) {
      return 'You must enter a value';
    }

    if (this.username.hasError('conflict')) {
      return 'Username is taken';
    }

    if (this.username.hasError('minlength')) {
      return 'Username too short';
    }

    if (this.username.hasError('maxlength')) {
      return 'Username too long';
    }

    return '';
  }

  getEmailErrorMessage() {
    if (this.email.hasError('required')) {
      return 'You must enter a value';
    }

    if (this.email.hasError('conflict')) {
      return 'Email is taken';
    }

    return this.email.hasError('email') ? 'Not a valid email' : '';
  }

  getPasswordErrorMessage() {
    if (this.password.hasError('required')) {
      return 'You must enter a value';
    }

    return this.password.hasError('minlength') ? 'Password too short' : '';
  }
}