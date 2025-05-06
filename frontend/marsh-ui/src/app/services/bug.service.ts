import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Bug } from '../models/bug.model';

@Injectable({
  providedIn: 'root',
})
export class BugService {
  private baseUrl = 'https://localhost:7213/api';
  private http = inject(HttpClient);

  getBugs(): Observable<Bug[]> {
    return this.http.get<Bug[]>(`${this.baseUrl}/bugs`);
  }
}
