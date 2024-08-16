import { Component, EventEmitter, Output, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthenticationService } from "@app/services/authentication.service";

@Component({
  selector: 'f-sign-up-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './sign-up-form.component.html'
})
export class SignUpFormComponent {
  @Output()
  registered = new EventEmitter();

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
    this.authenticationService.register(form.email ?? "", form.password ?? "")
      .subscribe({
        next: _ => this.registered.emit(),
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
