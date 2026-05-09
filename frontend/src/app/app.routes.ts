import { Routes } from '@angular/router';
import { APP_PATHS } from '@core/constants';

export const routes: Routes = [
  {
    path: APP_PATHS.AUTH.ROOT,
    loadChildren: () => import('./features/auth/auth.routes').then(f => f.AUTH_ROUTES),
  },
  { path: '', redirectTo: APP_PATHS.AUTH.ROOT, pathMatch: 'full' },
];
