export const APP_ROUTES_SEGMENTS = {
  EMPTY: '',
  AUTH: {
    LOGIN: 'login',
    SIGNUP: 'signup',
  },
  ADMIN: {
    AGENDA: 'agenda',
    CLIENTS: 'clients',
    SERVICES: 'services',
    SETTINGS: 'settings',
  },
} as const;

export const APP_ROUTES = {
  HOME: `/${APP_ROUTES_SEGMENTS.EMPTY}`,
  AUTH: {
    LOGIN: `/${APP_ROUTES_SEGMENTS.AUTH.LOGIN}`,
    SIGNUP: `/${APP_ROUTES_SEGMENTS.AUTH.SIGNUP}`,
  },
  ADMIN: {
    AGENDA: `/${APP_ROUTES_SEGMENTS.ADMIN.AGENDA}`,
    CLIENTS: `/${APP_ROUTES_SEGMENTS.ADMIN.CLIENTS}`,
    SERVICES: `/${APP_ROUTES_SEGMENTS.ADMIN.SERVICES}`,
    SETTINGS: `/${APP_ROUTES_SEGMENTS.ADMIN.SETTINGS}`,
  },
} as const;
