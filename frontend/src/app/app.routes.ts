import { Routes } from '@angular/router';
import { roleMatchGuard } from '@core/auth/guards';
import { APP_ROUTES_SEGMENTS } from '@core/constants';
import { UserRole } from '@core/shared';

export const routes: Routes = [
  {
    path: APP_ROUTES_SEGMENTS.EMPTY,
    loadChildren: () => import('./features/clients/client.routes').then(f => f.CLIENT_ROUTES),
    canMatch: [roleMatchGuard],
    data: { roles: [UserRole.Client] },
  },
  {
    path: APP_ROUTES_SEGMENTS.EMPTY,
    loadChildren: () => import('./features/admin/admin.routes').then(f => f.ADMIN_ROUTES),
    canMatch: [roleMatchGuard],
    data: { roles: [UserRole.Admin] },
  },
  {
    path: APP_ROUTES_SEGMENTS.AUTH.LOGIN,
    loadComponent: () => import('./features/auth/login/login').then(f => f.Login),
  },
  {
    path: APP_ROUTES_SEGMENTS.AUTH.SIGNUP,
    loadComponent: () => import('./features/auth/sign-up/sign-up').then(f => f.SignUp),
  },
  {
    path: APP_ROUTES_SEGMENTS.EMPTY,
    loadComponent: () => import('./features/home/home').then(f => f.Home),
  },
  { path: '**', redirectTo: APP_ROUTES_SEGMENTS.EMPTY, pathMatch: 'full' },
];
