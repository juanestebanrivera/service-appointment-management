export interface ClientAppointmentResponse {
  id: string;
  startAt: string;
  endAt: string;
  status: string;
  price: number;
  service: {
    serviceId: string;
    serviceName: string;
  };
}

export interface ClientUpcomingAppointmentResponse {
  nextAppointment: ClientAppointmentResponse | null;
  lastAppointment: ClientAppointmentResponse | null;
}
