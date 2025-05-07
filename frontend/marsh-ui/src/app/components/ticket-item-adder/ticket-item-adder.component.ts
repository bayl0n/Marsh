import { Component, output, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CreateTicketDto, Ticket } from '../../models/ticket.model';

@Component({
  selector: 'app-ticket-item-adder',
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
  templateUrl: './ticket-item-adder.component.html',
  styleUrl: './ticket-item-adder.component.scss',
})
export class TicketItemAdderComponent {
  readonly titleValue = signal('');
  readonly descriptionValue = signal('');
  addContent = signal(false);
  notifyAddTicket = output<CreateTicketDto>();

  onTitleInput(event: Event) {
    this.titleValue.set((event.target as HTMLInputElement).value);
  }

  onDescriptionInput(event: Event) {
    this.descriptionValue.set((event.target as HTMLInputElement).value);
  }

  onAddTicket() {
    let newTicket: CreateTicketDto = {
      title: this.titleValue(),
      description: this.descriptionValue(),
    };

    this.notifyAddTicket.emit(newTicket);
    this.hideAddContent();
  }

  toggleAddContent() {
    this.addContent.set(!this.addContent());
  }

  hideAddContent() {
    this.addContent.set(false);
    this.titleValue.set('');
    this.descriptionValue.set('');
  }
}
