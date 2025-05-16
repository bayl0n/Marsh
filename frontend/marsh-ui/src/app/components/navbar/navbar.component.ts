import { Component, effect, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink } from '@angular/router';
import { DebugButtonComponent } from '../debug-button/debug-button.component';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { MarshUser } from '../../models/user.model';

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
  readonly firebaseUser;
  appUser = signal<MarshUser | null>(null);

  constructor(
    private authService: AuthService,
    private userService: UserService
  ) {
    this.firebaseUser = toSignal(this.authService.user$, {
      initialValue: null,
    });

    userService.getCurrentUser().subscribe({
      next: (user) => {
        this.appUser.set(user);
      },
      error: (error) => {
        console.log(error);
      },
    });

    effect(() => {
      console.log('User changed:', this.firebaseUser());
    });
  }

  OnSignOut() {
    this.authService.logout();
  }
}
