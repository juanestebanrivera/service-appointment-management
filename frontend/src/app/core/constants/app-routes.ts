export const APP_PATHS = {
  AUTH: {
    ROOT: 'auth',
    LOGIN: 'login',
  },
} as const;

export const APP_ROUTES = {
  LOGIN: `/${APP_PATHS.AUTH.ROOT}/${APP_PATHS.AUTH.LOGIN}`,
} as const;
