import { Component, EventEmitter, Output, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthenticationService } from "@app/services/authentication.service";

@Component({
  selector: 'f-sign-in-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './sign-in-form.component.html'
})
export class SignInFormComponent {
  @Output()
  loggedIn = new EventEmitter();

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

  private authenticationService = inject(AuthenticationService)

  onSubmit() {
    const form = this.form.value;
    this.authenticationService.authenticate(form.email ?? "", form.password ?? "")
      .subscribe({
        next: _ => this.loggedIn.emit(),
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
