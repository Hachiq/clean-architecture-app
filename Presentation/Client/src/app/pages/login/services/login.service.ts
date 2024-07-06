import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest } from '../interfaces/login.request';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private http: HttpClient) { }

  public login(user: LoginRequest): Observable<string> {
    return this.http.post(`${environment.apiUrl}/auth/login`, user, { responseType: 'text', withCredentials: true });
  }

  public parseErrorResponse(e: any): any {
    const error = e.error;

    if (typeof error === 'string') {
      try {
        return JSON.parse(error);
      } catch (e) {
        console.error('Error parsing JSON:', e);
        return { reason: 'Unknown error' };
      }
    }
    
    return error;
  }
}
