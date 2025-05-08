import { Component, input, OnInit, output, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';
import { Ticket } from '../../models/ticket.model';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-ticket-item',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatTooltipModule,
    MatMenuModule,
    MatFormFieldModule,
    MatInputModule,
  ],
  templateUrl: './ticket-item.component.html',
  styleUrl: './ticket-item.component.scss',
})
export class TicketItemComponent implements OnInit {
  ticket = input.required<Ticket>();

  toggleEdit = signal(false);
  editTitleValue = signal('');
  editDescriptionValue = signal('');

  notifyDelete = output<number>();
  notifyToggleResolve = output<Ticket>();
  notifyEdit = output<Ticket>();

  ngOnInit(): void {
    this.editTitleValue.set(this.ticket().title);
    this.editDescriptionValue.set(this.ticket().description);
  }

  onDelete() {
    this.notifyDelete.emit(this.ticket().id);
  }

  onToggleEdit() {
    this.toggleEdit.set(!this.toggleEdit());
  }

  onEdit() {
    let editedTicket: Ticket = {
      ...this.ticket(),
      title: this.editTitleValue(),
      description: this.editDescriptionValue(),
    };
    this.notifyEdit.emit(editedTicket);
    this.onToggleEdit();
  }

  onEditTitle(event: Event) {
    this.editTitleValue.set((event.target as HTMLInputElement).value);
  }

  onEditDescription(event: Event) {
    this.editDescriptionValue.set((event.target as HTMLInputElement).value);
  }

  onToggleResolve() {
    let updatedBug: Ticket = {
      ...this.ticket(),
      isResolved: !this.ticket().isResolved,
    };
    this.notifyToggleResolve.emit(updatedBug);
  }
}
