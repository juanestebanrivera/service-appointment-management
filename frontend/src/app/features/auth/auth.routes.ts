import { Routes } from '@angular/router';
import { Login } from './login/login';
import { APP_PATHS } from '@core/constants';

export const AUTH_ROUTES: Routes = [
  { path: APP_PATHS.AUTH.LOGIN, component: Login },
  { path: '', redirectTo: APP_PATHS.AUTH.LOGIN, pathMatch: 'full' },
];
