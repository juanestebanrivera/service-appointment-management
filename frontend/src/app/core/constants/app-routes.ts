export const APP_ROUTES_SEGMENTS = {
  EMPTY: '',
  AUTH: {
    LOGIN: 'login',
    SIGNUP: 'signup',
  },
  ADMIN: {
    HOME: 'home',
    CLIENTS: 'clients',
    SERVICES: 'services',
    SETTINGS: 'settings',
  },
} as const;

export const APP_ROUTES = {
  AUTH: {
    LOGIN: `/${APP_ROUTES_SEGMENTS.AUTH.LOGIN}`,
    SIGNUP: `/${APP_ROUTES_SEGMENTS.AUTH.SIGNUP}`,
  },
  ADMIN: {
    HOME: `/${APP_ROUTES_SEGMENTS.ADMIN.HOME}`,
    CLIENTS: `/${APP_ROUTES_SEGMENTS.ADMIN.CLIENTS}`,
    SERVICES: `/${APP_ROUTES_SEGMENTS.ADMIN.SERVICES}`,
    SETTINGS: `/${APP_ROUTES_SEGMENTS.ADMIN.SETTINGS}`,
  },
} as const;
