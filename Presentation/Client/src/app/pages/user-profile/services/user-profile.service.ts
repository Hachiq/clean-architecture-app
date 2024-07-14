import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserProfile } from '../interfaces/user-profile';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { UserContactsRequest } from '../interfaces/user-contacts.request';

@Injectable({
  providedIn: 'root'
})
export class UserProfileService {

  constructor(private http: HttpClient) { }

  public getUser(id: string): Observable<UserProfile>{
    return this.http.get<UserProfile>(`${environment.apiUrl}/users/${id}`, { withCredentials: true });
  }

  public updateContacts(id: string, contacts: UserContactsRequest): Observable<void>{
    return this.http.put<void>(`${environment.apiUrl}/users/${id}/update-contacts`, contacts)
  }
}
