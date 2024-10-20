import { Component, inject, OnInit } from '@angular/core';
import { TicketService } from '../../services/ticket.service';
import { TicketRequest } from '../../models/ticket-request.model';
import { TicketResponse } from '../../models/ticket-response.model';
import { ApiResponse } from '../../models/api-response.model';
import { TicketFormComponent } from '../ticket-form/ticket-form.component';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [FormsModule, CommonModule, HttpClientModule, TicketFormComponent],
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
  message = "";
  errors: string[] = [];
  filterStatus: string = 'All';
  sortField: string = '';
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
          const paginatedTickets = response.data;
          this.tickets = paginatedTickets.items as TicketResponse[];
          this.filteredTickets = [...this.tickets];
          this.totalPages = paginatedTickets.totalPages;
          this.totalCount = paginatedTickets.totalCount;
        } else {
          console.error('Failed to load tickets:', response.errors);
        }
      },
      error: (error) => {
        console.error('Error fetching tickets:', error);
      }
    });
  }

  toggleAddTicketForm(): void {
    this.ticketDataForUpdate = undefined;
    this.addTicketFormVisible = !this.addTicketFormVisible;
  }

  handleTicketFormSubmit(ticketData: TicketResponse): void {
    const ticketRequest: TicketRequest = {
      ticketId: ticketData.ticketId,
      description: ticketData.description,
      status :(ticketData.status === "Open" ? 1 : (ticketData.status === "Closed" ? 2 : 0))
    };

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
      error: (error) => {
        console.error('Error saving ticket:', error);
      }
    });
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
        error: (error) => {
          console.error('Error deleting ticket:', error);
        }
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
    this.sortTickets(this.sortField); // Apply sorting after filtering
  }

  sortTickets(field: string): void {
    if (this.sortField === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortDirection = 'asc';
    }

    this.filteredTickets.sort((a, b) => {
      let valueA: any, valueB: any;

      switch (field) {
        case 'ticketId':
          valueA = a.ticketId;
          valueB = b.ticketId;
          break;
        case 'description':
          valueA = a.description.toLowerCase();
          valueB = b.description.toLowerCase();
          break;
        case 'status':
          valueA = a.status;
          valueB = b.status;
          break;
        case 'createdDate':
          valueA = new Date(a.createdDate);
          valueB = new Date(b.createdDate);
          break;
        default:
          return 0;
      }

      return this.sortDirection === 'asc'
        ? valueA < valueB ? -1 : 1
        : valueA > valueB ? -1 : 1;
    });
  }
}
