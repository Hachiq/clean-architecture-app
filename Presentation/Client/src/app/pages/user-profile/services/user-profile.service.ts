import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserProfile } from '../interfaces/user-profile';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  constructor(private http: HttpClient) { }

  public getUser(id: string): Observable<UserProfile>{
    return this.http.get<UserProfile>(`${environment.apiUrl}/users/${id}`);
  }
}
