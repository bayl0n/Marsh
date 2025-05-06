import { Component, signal } from '@angular/core';
import { BugService } from '../../services/bug.service';
import { Bug } from '../../models/bug.model';
import { MatCardModule } from '@angular/material/card';
import { BugCardComponent } from '../bug-card/bug-card.component';

@Component({
  selector: 'app-bug-list',
  imports: [MatCardModule, BugCardComponent],
  templateUrl: './bug-list.component.html',
  styleUrl: './bug-list.component.scss',
})
export class BugListComponent {
  bugs = signal<Bug[]>([]);

  constructor(private bugService: BugService) {}

  ngOnInit(): void {
    this.bugService.getBugs().subscribe({
      next: (data: Bug[]) => {
        this.bugs.set(data);
        console.log(data);
      },
      error: (err) => console.log(err),
    });
  }
}
