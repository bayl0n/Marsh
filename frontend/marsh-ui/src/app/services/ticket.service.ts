import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateTicketDto, Ticket } from '../models/ticket.model';

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  private baseUrl = 'http://10.0.0.66:5114/api/v1';
  private http = inject(HttpClient);

  getTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.baseUrl}/tickets`);
  }

  deleteTicket(id: number) {
    return this.http.delete(`${this.baseUrl}/tickets/${id}`);
  }

  putTicket(ticket: Ticket) {
    return this.http.put<Ticket>(`${this.baseUrl}/tickets`, ticket);
  }

  addTicket(ticket: CreateTicketDto) {
    return this.http.post<Ticket>(`${this.baseUrl}/tickets`, ticket);
  }
}
