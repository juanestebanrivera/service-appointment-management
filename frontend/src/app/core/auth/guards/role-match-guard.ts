import { inject } from '@angular/core';
import { CanMatchFn } from '@angular/router';
import { AuthState } from '@core/auth/services/auth-state';

export const roleMatchGuard: CanMatchFn = (route, segments) => {
  const authState = inject(AuthState);
  const userRole = authState.user()?.role;

  if (!userRole) return false;

  const allowedRoles = route.data?.['roles'] as Array<string>;

  if (!allowedRoles || allowedRoles.includes(userRole)) return true;

  return false;
};
