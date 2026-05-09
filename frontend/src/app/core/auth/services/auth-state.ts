import { computed, Injectable, signal } from '@angular/core';
import { User } from '../models';

@Injectable({
  providedIn: 'root',
})
export class AuthState {
  readonly #user = signal<User | null>(null);

  readonly user = this.#user.asReadonly();
  readonly isAuthenticated = computed(() => this.user() !== null);

  setUser(user: User): void {
    this.#user.set(user);
  }

  removeUser(): void {
    this.#user.set(null);
  }
}
