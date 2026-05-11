import { Routes } from '@angular/router';
import { UserRole } from '@core/auth/models';
import { APP_ROUTES_SEGMENTS } from '@core/constants';
import { roleMatchGuard } from '@core/guards/role-match-guard';

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
    path: APP_ROUTES_SEGMENTS.EMPTY,
    loadChildren: () => import('./features/auth/auth.routes').then(f => f.AUTH_ROUTES),
  },
  { path: '**', redirectTo: APP_ROUTES_SEGMENTS.EMPTY },
];
