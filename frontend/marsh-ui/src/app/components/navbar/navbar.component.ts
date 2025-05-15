import { Component, effect } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink } from '@angular/router';
import { DebugButtonComponent } from '../debug-button/debug-button.component';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [
    RouterLink,
    MatToolbarModule,
    MatButtonModule,
    DebugButtonComponent,
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  readonly user;

  constructor(private authService: AuthService) {
    this.user = toSignal(this.authService.user$, { initialValue: null });

    effect(() => {
      console.log('User changed:', this.user());
    });
  }

  OnSignOut() {
    this.authService.logout();
  }
}
