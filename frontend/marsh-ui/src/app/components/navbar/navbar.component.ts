import { Component, effect, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterLink } from '@angular/router';
import { DebugButtonComponent } from '../debug-button/debug-button.component';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { MarshUser } from '../../models/user.model';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-navbar',
  imports: [
    RouterLink,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
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

    effect(() => {
      console.log('User changed:', this.firebaseUser());
    });
  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.userService.getCurrentUser().subscribe({
      next: (user) => {
        this.appUser.set(user);
      },
      error: (error) => {
        console.log(error);
      },
    });
  }

  OnSignOut() {
    this.authService.logout();
  }
}
