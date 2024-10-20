import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response.model';
import { TicketRequest } from '../models/ticket-request.model';

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
   addTicket(ticketRequest: TicketRequest): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(this.apiUrl, ticketRequest);
  }

  // Update an existing ticket
  updateTicket(ticketRequest: TicketRequest): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}/${ticketRequest.ticketId}`, ticketRequest);
  }

  // Delete a ticket
  deleteTicket(id: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}/${id}`);
  }
}
