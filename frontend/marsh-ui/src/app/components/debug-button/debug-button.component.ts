import { Component } from '@angular/core';
import { Auth } from '@angular/fire/auth';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-debug-button',
  imports: [MatButtonModule],
  templateUrl: './debug-button.component.html',
  styleUrl: './debug-button.component.scss',
})
export class DebugButtonComponent {
  constructor(private auth: Auth) {}

  onClick() {
    this.auth.currentUser?.getIdToken(true).then((token) => {
      console.log('✅ Firebase ID Token:', token);
      // Optional: copy to clipboard
      navigator.clipboard.writeText(token);
    });

    if (this.auth.currentUser) console.log(this.auth.currentUser);
    else console.log('Current user is null.');
  }
}
