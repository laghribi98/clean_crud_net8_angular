import { Component, OnInit, inject } from '@angular/core';
import { TicketService } from '../../services/ticket.service';
import { TicketRequest } from '../../models/ticket-request.model';
import { TicketResponse } from '../../models/ticket-response.model';
import { ApiResponse } from '../../models/api-response.model';
import { TicketFormComponent } from '../ticket-form/ticket-form.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';


@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [FormsModule, CommonModule, HttpClientModule, TicketFormComponent,FontAwesomeModule],
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.css'
})
export class TicketListComponent implements OnInit {
  tickets: TicketResponse[] = [];
  filteredTickets: TicketResponse[] = [];
  currentPage = 1;
  pageSize = 5;
  totalPages = 0;
  totalCount = 0;
  message = '';
  errors: string[] = [];
  filterStatus = 'All';
  sortField = '';
  sortDirection: 'asc' | 'desc' = 'asc';
  addTicketFormVisible = false;
  ticketDataForUpdate?: TicketResponse;

  private ticketService = inject(TicketService);

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.ticketService.getTickets(this.currentPage, this.pageSize).subscribe({
      next: (response: ApiResponse) => {
        if (response.status === 200 && response.data) {
          this.setTicketData(response.data);
        } else {
          console.error('Failed to load tickets:', response.errors);
        }
      },
      error: (error) => console.error('Error fetching tickets:', error),
    });
  }

  private setTicketData(paginatedTickets: any): void {
    this.tickets = paginatedTickets.items as TicketResponse[];
    this.filteredTickets = [...this.tickets];
    this.totalPages = paginatedTickets.totalPages;
    this.totalCount = paginatedTickets.totalCount;
  }

  toggleAddTicketForm(): void {
    this.ticketDataForUpdate = undefined;
    this.addTicketFormVisible = !this.addTicketFormVisible;
  }

  handleTicketFormSubmit(ticketData: TicketResponse): void {
    const ticketRequest: TicketRequest = this.createTicketRequest(ticketData);

    const action$ = ticketRequest.ticketId
      ? this.ticketService.updateTicket(ticketRequest)
      : this.ticketService.addTicket(ticketRequest);

    action$.subscribe({
      next: (response) => {
        if (response.status === 200 || response.status === 201) {
          this.message = response.message;
          this.loadTickets();
          this.addTicketFormVisible = false;
        }
      },
      error: (error) => console.error('Error saving ticket:', error),
    });
  }

  private createTicketRequest(ticketData: TicketResponse): TicketRequest {
    return {
      ticketId: ticketData.ticketId,
      description: ticketData.description,
      status: ticketData.status === 'Open' ? 1 : ticketData.status === 'Closed' ? 2 : 0,
    };
  }

  handleTicketFormCancel(): void {
    this.addTicketFormVisible = false;
  }

  handleUpdate(ticket: TicketResponse): void {
    this.ticketDataForUpdate = ticket;
    this.addTicketFormVisible = true;
  }

  handleDelete(ticketId: number): void {
    if (confirm('Are you sure you want to delete this ticket?')) {
      this.ticketService.deleteTicket(ticketId).subscribe({
        next: (response) => {
          if (response.status === 200) {
            this.message = response.message;
            this.loadTickets();
          } else {
            console.error('Failed to delete ticket:', response.errors);
          }
        },
        error: (error) => console.error('Error deleting ticket:', error),
      });
    }
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

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadTickets();
    }
  }

  getPageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, i) => i + 1);
  }

  // Filtering and Sorting Tickets
  filterTickets(): void {
    this.filteredTickets = this.filterStatus === 'All'
      ? [...this.tickets]
      : this.tickets.filter(ticket => ticket.status === this.filterStatus);
    this.sortTickets(this.sortField);
  }

  sortTickets(field: string): void {
    this.sortField = this.sortField === field ? field : field;
    this.sortDirection = this.sortField === field ? this.toggleSortDirection() : 'asc';

    this.filteredTickets.sort((a, b) => this.compareTickets(a, b, field));
  }

  private toggleSortDirection(): 'asc' | 'desc' {
    return this.sortDirection === 'asc' ? 'desc' : 'asc';
  }

  private compareTickets(a: TicketResponse, b: TicketResponse, field: string): number {
    const valueA = this.extractFieldValue(a, field);
    const valueB = this.extractFieldValue(b, field);
    
    return this.sortDirection === 'asc'
      ? valueA < valueB ? -1 : 1
      : valueA > valueB ? -1 : 1;
  }

  private extractFieldValue(ticket: TicketResponse, field: string): any {
    switch (field) {
      case 'ticketId': return ticket.ticketId;
      case 'description': return ticket.description.toLowerCase();
      case 'status': return ticket.status;
      case 'createdDate': return new Date(ticket.createdDate);
      default: return 0;
    }
  }
}
