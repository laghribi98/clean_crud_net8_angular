import { Component, inject, OnInit } from '@angular/core';
import { Ticket } from '../../models/ticket.model';
import { TicketService } from '../../services/ticket.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { PaginatedTickets } from '../../models/paginated-tickets';
import { ApiResponse } from '../../models/tickets-response';

@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [FormsModule,CommonModule,HttpClientModule],
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.css'
})
export class TicketListComponent implements OnInit {
  message = "";
  errors : string[] = [];
  tickets: Ticket[] = [];
  currentPage: number = 1;
  pageSize: number = 5;
  totalCount: number = 0;
  totalPages: number = 0;
  filteredTickets: Ticket[] = []; // Holds the tickets after filtering/sorting

   // Sorting and Filtering Variables
   filterStatus: string = 'All'; // Filter for ticket status
   sortField: string = ''; // Field to sort by (e.g., 'description' or 'createdDate')
   sortDirection: 'asc' | 'desc' = 'asc'; // Direction of sort ('asc' or 'desc')

  private ticketService = inject(TicketService);

  ngOnInit(): void {
    this.loadTickets();
    console.log(this.tickets)
  }

  loadTickets(): void {
    this.ticketService.getTickets(this.currentPage, this.pageSize).subscribe(
      (response: ApiResponse) => {
        this.message = response.message;
        this.errors = response.errors;
        const paginatedTickets = response.data;      
        this.tickets = paginatedTickets.items;
        this.filteredTickets = [...this.tickets]; // Initialize filtered tickets
        this.currentPage = paginatedTickets.pageNumber;
        this.pageSize = paginatedTickets.pageSize;
        this.totalCount = paginatedTickets.totalCount;
        this.totalPages = paginatedTickets.totalPages;
      },
      (error) => {
        console.error('Error fetching tickets:', error);
      }
    );
  }

  // Pagination methods
  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadTickets();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadTickets();
    }
  }

   // Filtering Tickets by Status
   filterTickets(): void {
    if (this.filterStatus === 'All') {
      this.filteredTickets = [...this.tickets];
    } else {
      this.filteredTickets = this.tickets.filter(ticket => ticket.status === this.filterStatus);
    }
    this.sortTickets(this.sortField); // Apply sorting after filtering
  }

  // Sorting Tickets
  sortTickets(field: string): void {
    if (this.sortField === field) {
      // Toggle direction if the same field is clicked
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      // Set new field to sort by and default to ascending
      this.sortField = field;
      this.sortDirection = 'asc';
    }
  
    this.filteredTickets.sort((a, b) => {
      let valueA: any;
      let valueB: any;
  
      // Use a switch or if-else block to handle specific fields
      if (field === 'ticketId') {
        valueA = a.ticketId;
        valueB = b.ticketId;
      } else if (field === 'description') {
        valueA = a.description.toLowerCase(); // Sort ignoring case
        valueB = b.description.toLowerCase();
      } else if (field === 'status') {
        valueA = a.status;
        valueB = b.status;
      } else if (field === 'createdDate') {
        valueA = new Date(a.createdDate);
        valueB = new Date(b.createdDate);
      } else {
        return 0; // Return 0 if the field is not recognized
      }
  
      if (valueA < valueB) {
        return this.sortDirection === 'asc' ? -1 : 1;
      }
      if (valueA > valueB) {
        return this.sortDirection === 'asc' ? 1 : -1;
      }
      return 0;
    });
  }
  
}
