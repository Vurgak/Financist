import { Component, inject } from '@angular/core';
import {Router, RouterOutlet } from '@angular/router';
import {AuthenticationService} from "@app/services/authentication.service";

@Component({
  selector: 'f-main-layout',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './main-layout.component.html'
})
export class MainLayoutComponent {
  private router = inject(Router);
  private authenticationService = inject(AuthenticationService);

  onSignOut() {
    this.authenticationService.logOut()
      .subscribe({
        complete: () => this.router.navigate(["/"]),
      });
  }
}
