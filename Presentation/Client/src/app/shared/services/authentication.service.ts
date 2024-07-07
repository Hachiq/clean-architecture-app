import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  
  constructor(private http: HttpClient) { }

  private accessToken: string | null = null;

  private currentUserSource = new ReplaySubject<User | null>(1);
  currentUser$ = this.currentUserSource.asObservable();

  loadCurrentUser(){
    return this.http.get<User>(`${environment.apiUrl}/auth/get-current-user`,  { withCredentials: true }).pipe(
      map(user => {
        this.currentUserSource.next(user);
      })
    )
  }

  removeCurrentUser(){
    this.currentUserSource.next(null);
  }

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
