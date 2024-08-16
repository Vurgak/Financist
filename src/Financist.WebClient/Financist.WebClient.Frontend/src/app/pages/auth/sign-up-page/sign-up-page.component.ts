import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import {AuthenticationService} from "@app/services/authentication.service";

@Component({
  selector: 'f-sign-up-page',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './sign-up-page.component.html'
})
export class SignUpPageComponent {
  private authenticationService = inject(AuthenticationService)
  private router = inject(Router);

  form = new FormGroup({
    email: new FormControl('', [
      Validators.required,
      Validators.email,
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
    ]),
  });

  hasError = false;

  onSubmit() {
    const form = this.form.value;
    this.authenticationService.register(form.email ?? "", form.password ?? "")
      .subscribe({
        next: _ => this.router.navigate(["/sign-in"]),
        error: _ => this.hasError = true,
      });
  }

  get email() {
    return this.form.get('email')!;
  }

  get password() {
    return this.form.get('password')!;
  }
}
