import { Component, signal } from '@angular/core';
import { TicketService } from '../../services/ticket.service';
import { CreateTicketDto, Ticket } from '../../models/ticket.model';
import { MatCardModule } from '@angular/material/card';
import { TicketItemComponent } from '../ticket-item/ticket-item.component';
import {
  CdkDragDrop,
  moveItemInArray,
  transferArrayItem,
  CdkDrag,
  CdkDropList,
} from '@angular/cdk/drag-drop';
import { TicketItemAdderComponent } from '../ticket-item-adder/ticket-item-adder.component';

@Component({
  selector: 'app-ticket-list',
  imports: [
    MatCardModule,
    TicketItemComponent,
    CdkDropList,
    CdkDrag,
    TicketItemAdderComponent,
  ],
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.scss',
})
export class TicketListComponent {
  tickets: Ticket[] = [];

  constructor(private bugService: TicketService) {}

  ngOnInit(): void {
    this.bugService.getTickets().subscribe({
      next: (data: Ticket[]) => {
        this.tickets = data;
      },
      error: (err) => console.log(err),
    });
  }

  onAddTicket(ticket: CreateTicketDto) {
    this.bugService.addTicket(ticket).subscribe({
      next: (ticket) => {
        this.tickets.push(ticket);
      },
      error: (err) => console.log(err),
    });
  }

  onDeleteTicket(id: number) {
    this.bugService.deleteTicket(id).subscribe({
      next: () => {
        this.tickets = this.tickets.filter((b) => b.id !== id);
      },
      error: (err) => console.log(err),
    });
  }

  onEditTicket(ticket: Ticket) {
    this.bugService.putTicket(ticket).subscribe({
      next: (newTicket) => {
        let index = this.tickets.findIndex((item) => item.id === newTicket.id);

        this.tickets[index] = newTicket;
      },
      error: (err) => console.log(err),
    });
  }

  onToggleResolve(ticket: Ticket) {
    this.bugService.putTicket(ticket).subscribe({
      next: (newTicket: Ticket) => {
        let index = this.tickets.findIndex((item) => item.id === newTicket.id);

        this.tickets[index] = newTicket;
      },
      error: (err) => console.log(err),
    });
  }

  drop(event: CdkDragDrop<Ticket[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    } else {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );
    }
  }
}
