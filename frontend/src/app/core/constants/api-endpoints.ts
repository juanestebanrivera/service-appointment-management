export const API_BASE_URL = 'http://localhost:8080/api/v1';

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

export const CLIENT_ENDPOINTS = {
  GET_ALL: '/clients',
  GET_BY_ID: (id: string) => `/clients/${id}`,
  CREATE: '/clients',
  UPDATE: (id: string) => `/clients/${id}`,
  DELETE: (id: string) => `/clients/${id}`,
  APPOINTMENTS: {
    GET_ALL: (clientId: string) => `/clients/${clientId}/appointments`,
    UPCOMING: (clientId: string) => `/clients/${clientId}/appointments/upcoming`,
  },
};
