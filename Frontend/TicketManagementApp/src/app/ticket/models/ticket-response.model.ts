export interface TicketResponse {
    ticketId: number;
    description: string;
    status: 'Open' | 'Closed';
    createdDate: string; 
  }
  