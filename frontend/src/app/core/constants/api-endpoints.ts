export const API_BASE_URL = '';

export const AUTH_ENDPOINTS = {
  LOGIN: '/auth/token',
  SIGNUP: '/auth/signup',
};

export const SERVICE_ENDPOINTS = {
  GET_ALL: '/services',
  GET_BY_ID: (id: string) => `/services/${id}`,
  CREATE: '/services',
  UPDATE: (id: string) => `/services/${id}`,
  DELETE: (id: string) => `/services/${id}`,
};
