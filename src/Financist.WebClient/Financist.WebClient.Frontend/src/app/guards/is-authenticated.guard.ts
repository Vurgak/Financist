import { CanActivateFn, Router } from "@angular/router";
import { AuthenticationService } from "@app/services/authentication.service";
import { inject } from "@angular/core";
import { catchError, map, of } from "rxjs";

export const isAuthenticatedGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authenticationService = inject(AuthenticationService);

  return authenticationService.getUserSession()
    .pipe(
      map(_ => true),
      catchError(_ => {
        router.navigate(["/sign-in"]);
        return of(false);
      }),
    );
};
