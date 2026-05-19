export const API_BASE_URL = 'http://localhost:8080/api/v1';

export const AUTH_ENDPOINTS = {
  LOGIN: '/auth/token',
  SIGNUP: '/auth/signup',
};

export const USER_ENDPOINTS = {
  GET_BY_ID: (id: string) => `/users/${id}`,
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

export const APPOINTMENT_ENDPOINTS = {
  GET_ALL: `/appointments`,
  GET_BY_ID: (id: string) => `/appointments/${id}`,
  CREATE: '/appointments',
  RESCHEDULE: (id: string) => `/appointments/${id}/reschedule`,
  CANCEL: (id: string) => `/appointments/${id}/cancel`,
  COMPLETE: (id: string) => `/appointments/${id}/complete`,
  CONFIRM: (id: string) => `/appointments/${id}/confirm`,
  NO_SHOW: (id: string) => `/appointments/${id}/no-show`,
};
