import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { MarshUser } from '../models/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private baseUrl = environment.baseUrl;
  private http = inject(HttpClient);

  getCurrentUser() {
    return this.http.get<MarshUser>(`${this.baseUrl}/users/me`);
  }
}
