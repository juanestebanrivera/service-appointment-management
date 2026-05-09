import { isPlatformServer } from '@angular/common';
import { HttpInterceptorFn } from '@angular/common/http';
import { inject, PLATFORM_ID } from '@angular/core';
import { AuthTokenStorage } from '@core/auth/services/auth-token-storage';
import { AUTH_ENDPOINTS } from '@core/constants';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  if (request.url.includes(AUTH_ENDPOINTS.LOGIN)) return next(request);

  const platformId = inject(PLATFORM_ID);

  if (isPlatformServer(platformId)) return next(request);

  const token = inject(AuthTokenStorage).getToken();

  if (!token) return next(request);

  const newRequest = request.clone({
    setHeaders: { Authorization: `Bearer ${token}` },
  });

  return next(newRequest);
};
