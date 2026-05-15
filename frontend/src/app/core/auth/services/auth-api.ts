import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { API_BASE_URL, APP_ROUTES, AUTH_ENDPOINTS } from '@core/constants';
import { AuthRequest, AuthResponse } from '../models';
import { AuthTokenStorage } from './auth-token-storage';
import { getErrorMessage } from '@core/utils/error-handler';
import { AuthState } from './auth-state';
import { SignUpRequest } from '../models/signup.model';

@Injectable({
  providedIn: 'root',
})
export class AuthApi {
  readonly #http = inject(HttpClient);
  readonly #router = inject(Router);
  readonly #authTokenStorage = inject(AuthTokenStorage);
  readonly #authState = inject(AuthState);

  login(credentials: AuthRequest): Observable<AuthResponse> {
    return this.#http
      .post<AuthResponse>(`${API_BASE_URL}${AUTH_ENDPOINTS.LOGIN}`, credentials)
      .pipe(
        tap(res => {
          this.#authTokenStorage.saveToken(res.token);
        }),
        catchError((errorResponse: HttpErrorResponse) => {
          const errorMessage = getErrorMessage(errorResponse);

          return throwError(() => new Error(errorMessage));
        }),
      );
  }

  signUp(request: SignUpRequest): Observable<void> {
    return this.#http.post<void>(`${API_BASE_URL}${AUTH_ENDPOINTS.SIGNUP}`, request).pipe(
      catchError((errorResponse: HttpErrorResponse) => {
        const errorMessage = getErrorMessage(errorResponse);

        return throwError(() => new Error(errorMessage));
      }),
    );
  }

  logout(): void {
    this.#authTokenStorage.destroyToken();
    this.#authState.removeUser();

    this.#router.navigate([APP_ROUTES.AUTH.LOGIN]);
  }
}
