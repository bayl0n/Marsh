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
import { of, switchMap } from 'rxjs';
import { AsyncPipe, CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  imports: [
    CommonModule,
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
  readonly appUser$;

  constructor(
    private authService: AuthService,
    private userService: UserService
  ) {
    this.firebaseUser = toSignal(this.authService.user$, {
      initialValue: null,
    });

    this.appUser$ = this.authService.user$.pipe(
      switchMap((fbUser) =>
        fbUser ? this.userService.getCurrentUser() : of<MarshUser | null>(null)
      )
    );

    effect(() => {
      console.log('User changed:', this.firebaseUser());
    });
  }

  OnSignOut() {
    this.authService.logout();
  }
}
