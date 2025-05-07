import { Component } from '@angular/core';
import { TicketListComponent } from '../../components/ticket-list/ticket-list.component';
import { CdkDropListGroup } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-home',
  imports: [TicketListComponent, CdkDropListGroup],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {}
