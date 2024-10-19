import { PaginatedTickets } from "./paginated-tickets";

export interface ApiResponse {
    status: number;
    message: string;
    data: PaginatedTickets;
    errors: any;  // Can be null or contain an error object
  } 