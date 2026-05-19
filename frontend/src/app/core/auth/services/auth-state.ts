import { computed, inject, Injectable, PLATFORM_ID, signal } from '@angular/core';
import { User } from '../models';
import { isPlatformServer } from '@angular/common';

@Injectable({
  providedIn: 'root',
})
export class AuthState {
  readonly #user = signal<User | null>(null);
  readonly #STORAGE_KEY = 'user';

  readonly user = this.#user.asReadonly();
  readonly isAuthenticated = computed(() => this.user() !== null);

  constructor() {
    const platformId = inject(PLATFORM_ID);

    if (isPlatformServer(platformId)) return;

    const storedUser = localStorage.getItem(this.#STORAGE_KEY);

    if (storedUser) {
      this.#user.set(JSON.parse(storedUser));
    }
  }

  setUser(user: User): void {
    localStorage.setItem(this.#STORAGE_KEY, JSON.stringify(user));
    this.#user.set(user);
  }

  removeUser(): void {
    localStorage.removeItem(this.#STORAGE_KEY);
    this.#user.set(null);
  }
}
