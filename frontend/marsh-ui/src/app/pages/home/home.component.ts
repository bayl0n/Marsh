import { Component } from '@angular/core';
import { BugListComponent } from '../../components/bug-list/bug-list.component';

@Component({
  selector: 'app-home',
  imports: [BugListComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {}
