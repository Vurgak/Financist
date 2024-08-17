import { CanActivateFn, Router } from "@angular/router";
import { AuthenticationService } from "@app/services/authentication.service";
import { inject } from "@angular/core";
import { catchError, map, of } from "rxjs";

export const isUnauthenticatedGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authenticationService = inject(AuthenticationService);

  return authenticationService.getUserSession()
    .pipe(
      map(_ => {
        router.navigate(["/dashboard"]);
        return false;
      }),
      catchError(_ => of(true)),
    );
};
