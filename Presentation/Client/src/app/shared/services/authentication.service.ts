import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  
  constructor(private http: HttpClient) { }

  private accessToken: string | null = null;

  setToken(token: string): void {
    this.accessToken = token;
  }

  getToken(): string | null {
    return this.accessToken;
  }

  clearToken(): void {
    this.accessToken = null;
  }

  isAuthorized(): boolean {
    return !!this.accessToken;
  }

  refreshToken(): Observable<string> {
    return this.http.get(`${environment.apiUrl}/auth/refresh-token`, { responseType: 'text', withCredentials: true });
  }
}
