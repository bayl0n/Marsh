import { Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { Ticket } from '../../models/ticket.model';

@Component({
  selector: 'app-ticket-item',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatTooltipModule,
    MatMenuModule,
  ],
  templateUrl: './ticket-item.component.html',
  styleUrl: './ticket-item.component.scss',
})
export class TicketItemComponent {
  ticket = input.required<Ticket>();
  notifyDelete = output<number>();
  notifyToggleResolve = output<Ticket>();

  onDelete() {
    this.notifyDelete.emit(this.ticket().id);
  }

  onToggleResolve() {
    let updatedBug: Ticket = {
      ...this.ticket(),
      isResolved: !this.ticket().isResolved,
    };
    this.notifyToggleResolve.emit(updatedBug);
  }
}
