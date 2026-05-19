import { Component, inject } from '@angular/core';
import { Icon } from '@shared/components/icon/icon';
import { RouterLink } from '@angular/router';
import { APP_ROUTES } from '@core/constants';
import {
  FormControl,
  NonNullableFormBuilder,
  Validators,
  ɵInternalFormsSharedModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { AuthApi } from '@core/auth';
import { ClientsApi } from '@core/clients';

interface SignUpFormGroup {
  firstName: FormControl<string>;
  lastName: FormControl<string>;
  phonePrefix: FormControl<string>;
  phone: FormControl<string>;
  email: FormControl<string>;
  password: FormControl<string>;
  confirmPassword: FormControl<string>;
}

@Component({
  selector: 'app-sign-up',
  imports: [Icon, RouterLink, ɵInternalFormsSharedModule, ReactiveFormsModule],
  templateUrl: './sign-up.html',
})
export class SignUp {
  readonly ROUTES = APP_ROUTES;

  readonly #authApi = inject(AuthApi);
  readonly #clientApi = inject(ClientsApi);
  readonly #formBuilder = inject(NonNullableFormBuilder);

  signUpForm = this.#formBuilder.group<SignUpFormGroup>({
    firstName: this.#formBuilder.control<string>('', [Validators.required]),
    lastName: this.#formBuilder.control<string>('', [Validators.required]),
    phonePrefix: this.#formBuilder.control<string>('+57', [Validators.required]),
    phone: this.#formBuilder.control<string>('', [Validators.required]),
    email: this.#formBuilder.control<string>('', [Validators.required, Validators.email]),
    password: this.#formBuilder.control<string>('', [Validators.required]),
    confirmPassword: this.#formBuilder.control<string>('', [Validators.required]),
  });

  onSubmit() {
    if (!this.signUpForm.valid) return;

    const { firstName, lastName, phonePrefix, phone, email, password, confirmPassword } =
      this.signUpForm.value;

    if (password !== confirmPassword) {
      return;
    }

    // TODO: Register user in auth service and client in clients service, then navigate to login page
  }
}
