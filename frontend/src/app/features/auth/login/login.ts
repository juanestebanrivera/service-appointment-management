import { Component, inject } from '@angular/core';
import { AuthApi } from '../../../core/auth/services/auth-api';
import {
  FormControl,
  NonNullableFormBuilder,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { AuthRequest } from '@core/auth/models';

interface LoginFormGroup {
  email: FormControl<string>;
  password: FormControl<string>;
}

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
})
export class Login {
  readonly #auth = inject(AuthApi);
  readonly #formBuilder = inject(NonNullableFormBuilder);

  loginForm = this.#formBuilder.group<LoginFormGroup>({
    email: this.#formBuilder.control('', [Validators.required, Validators.email]),
    password: this.#formBuilder.control('', [Validators.required]),
  });

  onLogin() {
    if (!this.loginForm.valid) return;

    const { email, password } = this.loginForm.value;

    this.#auth.login({ email: email, password: password } as AuthRequest).subscribe();
  }
}
