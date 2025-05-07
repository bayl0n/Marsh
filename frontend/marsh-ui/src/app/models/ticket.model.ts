export interface Ticket {
  id: number;
  title: string;
  description: string;
  isResolved: boolean;
  createdAt: string;
}

export interface CreateTicketDto {
  title: string;
  description: string;
}
