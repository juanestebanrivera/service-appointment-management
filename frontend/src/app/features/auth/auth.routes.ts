import { Routes } from '@angular/router';
import { APP_ROUTES_SEGMENTS } from '@core/constants';

export const AUTH_ROUTES: Routes = [
  {
    path: APP_ROUTES_SEGMENTS.AUTH.LOGIN,
    loadComponent: () => import('./login/login').then(f => f.Login),
  },
  {
    path: APP_ROUTES_SEGMENTS.EMPTY,
    redirectTo: APP_ROUTES_SEGMENTS.AUTH.LOGIN,
    pathMatch: 'full',
  },
];
