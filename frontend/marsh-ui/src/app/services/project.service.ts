import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ProjectService {
  private baseUrl = environment.baseUrl;
  private http = inject(HttpClient);

  getCurrentProjects() {
    this.http.get<
  }
}
