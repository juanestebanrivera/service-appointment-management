import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, exhaustMap, map, Observable, tap } from 'rxjs';
import { API_BASE_URL, APP_ROUTES, AUTH_ENDPOINTS, USER_ENDPOINTS } from '@core/constants';
import { AuthTokenStorage } from './auth-token-storage';
import { returnThrowHttpErrorResponse } from '@core/utils/error-handler';
import { AuthState } from './auth-state';
import { AuthRequest, AuthResponse, SignUpRequest } from '../dtos';
import { User } from '../models';
import { mapUserResponseToUser } from '../mappers';

@Injectable({
  providedIn: 'root',
})
export class AuthApi {
  readonly #http = inject(HttpClient);
  readonly #router = inject(Router);
  readonly #authTokenStorage = inject(AuthTokenStorage);
  readonly #authState = inject(AuthState);

  login(credentials: AuthRequest): Observable<User | null> {
    return this.#http
      .post<AuthResponse>(`${API_BASE_URL}${AUTH_ENDPOINTS.LOGIN}`, credentials)
      .pipe(
        tap(response => this.#authTokenStorage.saveToken(response.token)),
        exhaustMap(response => this.#getUser(response.userId)),
        tap(user => this.#authState.setUser(user)),
        catchError(returnThrowHttpErrorResponse),
      );
  }

  signUp(request: SignUpRequest): Observable<void> {
    return this.#http
      .post<void>(`${API_BASE_URL}${AUTH_ENDPOINTS.SIGNUP}`, request)
      .pipe(catchError(returnThrowHttpErrorResponse));
  }

  logout(): void {
    this.#authTokenStorage.destroyToken();
    this.#authState.removeUser();

    this.#router.navigate([APP_ROUTES.AUTH.LOGIN]);
  }

  #getUser(id: string): Observable<User> {
    return this.#http
      .get<User>(`${API_BASE_URL}${USER_ENDPOINTS.GET_BY_ID(id)}`)
      .pipe(map(mapUserResponseToUser), catchError(returnThrowHttpErrorResponse));
  }
}
