import { Component } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button'
import { FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginService } from './services/login.service';
import { Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    HttpClientModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  constructor(private loginService: LoginService, private router: Router, private authService: AuthenticationService) {}

  hide = true;

  username = new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]);
  password = new FormControl('', [Validators.required, Validators.minLength(6)]);

  login() {
    this.loginService.login({
      username: this.username.value,
      password: this.password.value
    }).subscribe({
      next: (token) => {
        console.log(`User ${this.username.value} was logged in successfully`);
        this.authService.setToken(token);
        this.router.navigate(['home']);
      },
      error: (e) => {
        const parsedError = this.loginService.parseErrorResponse(e);

        if(e.status === 400){
          const reason = parsedError.reason;

          if (reason === 'NoMatch') {
            this.username.setErrors({ nomatch: true });
            this.password.setErrors({ nomatch: true });
          }
        }
        else {
          console.log(e);
        }
      }
    })
  }

  getUsernameErrorMessage() {
    if (this.username.hasError('required')) {
      return 'You must enter a value';
    }

    if (this.username.hasError('minlength')) {
      return 'Username too short';
    }

    if (this.username.hasError('maxlength')) {
      return 'Username too long';
    }

    return this.username.hasError('nomatch') ? 'User not found or wrong password' : '';
  }

  getPasswordErrorMessage() {
    if (this.password.hasError('required')) {
      return 'You must enter a value';
    }

    if (this.password.hasError('minlength')) {
      return 'Password too short';
    }

    return this.password.hasError('nomatch') ? '' : '';
  }
}
