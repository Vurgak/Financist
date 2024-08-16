import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SignInFormComponent } from "@app/components/auth/sign-in-form/sign-in-form.component";

@Component({
  selector: 'f-sign-in-page',
  standalone: true,
  imports: [SignInFormComponent],
  templateUrl: './sign-in-page.component.html'
})
export class SignInPageComponent {
  private router = inject(Router);

  onLoggedIn() {
    this.router.navigate(["/dashboard"]);
  }
}
