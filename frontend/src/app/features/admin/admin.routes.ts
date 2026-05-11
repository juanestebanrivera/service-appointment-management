import { Routes } from '@angular/router';
import { APP_ROUTES_SEGMENTS } from '@core/constants';

export const ADMIN_ROUTES: Routes = [
  {
    path: APP_ROUTES_SEGMENTS.EMPTY,
    loadComponent: () => import('../../layouts/admin-layout/admin-layout').then(f => f.AdminLayout),
    children: [
      {
        path: APP_ROUTES_SEGMENTS.ADMIN.HOME,
        loadComponent: () => import('./pages/home/home').then(f => f.Home),
      },
      {
        path: APP_ROUTES_SEGMENTS.ADMIN.CLIENTS,
        loadComponent: () => import('./pages/clients/clients').then(f => f.Clients),
      },
      {
        path: APP_ROUTES_SEGMENTS.ADMIN.SERVICES,
        loadComponent: () => import('./pages/services/services').then(f => f.Services),
      },
      {
        path: APP_ROUTES_SEGMENTS.ADMIN.SETTINGS,
        loadComponent: () => import('./pages/settings/settings').then(f => f.Settings),
      },
      {
        path: APP_ROUTES_SEGMENTS.EMPTY,
        pathMatch: 'full',
        redirectTo: APP_ROUTES_SEGMENTS.ADMIN.HOME,
      },
    ],
  },
];
