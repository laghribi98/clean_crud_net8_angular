export interface TicketRequest {
    ticketId?: number; // Optional for creation, required for update
    description: string;
    status: number; 
  }
  