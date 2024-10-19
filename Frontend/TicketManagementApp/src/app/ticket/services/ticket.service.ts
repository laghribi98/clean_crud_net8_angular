import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Ticket } from '../models/ticket.model';
import { PaginatedTickets } from '../models/paginated-tickets';
import { ApiResponse } from '../models/tickets-response';

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private apiUrl = 'http://localhost:5035/api/Tickets';

  constructor(private http: HttpClient) {}

  // Fetch all tickets
  getTickets(pageNumber: number = 1, pageSize: number = 10): Observable<ApiResponse> {
    return this.http.get<ApiResponse>(`${this.apiUrl}?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  // Add a new ticket
  addTicket(ticket: Ticket): Observable<Ticket> {
    return this.http.post<Ticket>(this.apiUrl, ticket);
  }

  // Update a ticket
  updateTicket(ticket: Ticket): Observable<Ticket> {
    return this.http.put<Ticket>(`${this.apiUrl}/${ticket.ticketId}`, ticket);
  }

  // Delete a ticket
  deleteTicket(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
