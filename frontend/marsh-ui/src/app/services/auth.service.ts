import {
  Injectable,
  EnvironmentInjector,
  runInInjectionContext,
} from '@angular/core';
import {
  Auth,
  signInWithEmailAndPassword,
  createUserWithEmailAndPassword,
  signOut,
  User,
  authState,
} from '@angular/fire/auth';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  readonly user$: Observable<User | null>;

  constructor(
    private auth: Auth,
    private injector: EnvironmentInjector // ← needed for runInInjectionContext
  ) {
    this.user$ = authState(this.auth);
  }

  login(email: string, password: string) {
    return runInInjectionContext(this.injector, () =>
      signInWithEmailAndPassword(this.auth, email, password)
    );
  }

  register(email: string, password: string) {
    return runInInjectionContext(this.injector, () =>
      createUserWithEmailAndPassword(this.auth, email, password)
    );
  }

  logout() {
    return runInInjectionContext(this.injector, () => signOut(this.auth));
  }

  getToken() {
    // reading a property is fine, but if you call other Firebase APIs here, wrap them too
    return this.auth.currentUser?.getIdToken();
  }
}
