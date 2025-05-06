import { Component, input, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { Bug } from '../../models/bug.model';

@Component({
  selector: 'app-bug-card',
  imports: [MatCardModule, MatButtonModule, MatIconModule, MatCheckboxModule],
  templateUrl: './bug-card.component.html',
  styleUrl: './bug-card.component.scss',
})
export class BugCardComponent {
  bug = input.required<Bug>();
}
