export interface Ticket {
  ticketId: number;
    description: string;
    status: 'Open' | 'Closed';
    createdDate: string;  // You can use Date if preferred
  }