import { Ticket } from "./ticket.model";

export interface PaginatedTickets {
    items: Ticket[];  // Array of tickets
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  }