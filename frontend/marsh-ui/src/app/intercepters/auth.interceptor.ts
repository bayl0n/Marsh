import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Auth, authState } from '@angular/fire/auth';
import { from } from 'rxjs';
import { switchMap, take } from 'rxjs/operators';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(Auth);

  // authState(auth) will emit `null` then your user (or vice versa),
  // so we take(1) to get whichever comes first.
  return authState(auth).pipe(
    take(1),
    switchMap((user) => {
      if (!user) {
        // no user yet (or not logged in) → just forward
        console.log('No user found from interceptor');
        return next(req);
      }
      // user exists → get fresh token and clone the request
      return from(user.getIdToken()).pipe(
        switchMap((token) =>
          next(
            req.clone({
              setHeaders: { Authorization: `Bearer ${token}` },
            })
          )
        )
      );
    })
  );
};
