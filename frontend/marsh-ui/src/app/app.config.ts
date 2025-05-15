import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideFirebaseApp, initializeApp, getApp } from '@angular/fire/app';
import {
  provideAuth,
  browserLocalPersistence,
  initializeAuth,
} from '@angular/fire/auth';
import { provideRouter } from '@angular/router';
import { environment } from '../environments/environment';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './intercepters/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideFirebaseApp(() => initializeApp(environment.firebase)),
    provideAuth(() =>
      // this waits for persistence under the hood
      initializeAuth(getApp(), {
        persistence: [browserLocalPersistence],
      })
    ),
    provideHttpClient(withInterceptors([authInterceptor])),
  ],
};
