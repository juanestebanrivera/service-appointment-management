import { Component, inject, signal } from '@angular/core';
import {
  FormControl,
  NonNullableFormBuilder,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Icon } from '@shared/components/icon/icon';
import { Router, RouterLink } from '@angular/router';
import { APP_ROUTES } from '@core/constants';
import { AuthApi } from '@core/auth';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { AuthRequest } from '@core/auth/dtos';

interface LoginFormGroup {
  email: FormControl<string>;
  password: FormControl<string>;
}

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, Icon, RouterLink],
  templateUrl: './login.html',
})
export class Login {
  readonly ROUTES = APP_ROUTES;

  readonly #authApi = inject(AuthApi);
  readonly #formBuilder = inject(NonNullableFormBuilder);
  readonly #router = inject(Router);

  errorMessage = signal<string>('');
  loginForm = this.#formBuilder.group<LoginFormGroup>({
    email: this.#formBuilder.control<string>('', [Validators.required, Validators.email]),
    password: this.#formBuilder.control<string>('', [Validators.required]),
  });

  onLogin() {
    this.errorMessage.set('');

    if (!this.loginForm.valid) return;

    const { email, password } = this.loginForm.value;

    this.#authApi.login({ email: email, password: password } as AuthRequest).subscribe({
      next: () => {
        this.#router.navigate([this.ROUTES.HOME]);
      },
      error: (err: HttpErrorResponse) => {
        if (err.status === HttpStatusCode.Unauthorized) {
          this.errorMessage.set('Correo electrónico o contraseña incorrectos');

          return;
        }

        this.errorMessage.set(
          'Ocurrió un error inesperado, por favor intente nuevamente más tarde',
        );
      },
    });
  }
}
