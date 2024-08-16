import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SignUpFormComponent } from "@app/components/auth/sign-up-form/sign-up-form.component";

@Component({
  selector: 'f-sign-up-page',
  standalone: true,
  imports: [SignUpFormComponent],
  templateUrl: './sign-up-page.component.html'
})
export class SignUpPageComponent {
  private router = inject(Router);

  onRegistered() {
    this.router.navigate(["/sign-in"]);
  }
}
