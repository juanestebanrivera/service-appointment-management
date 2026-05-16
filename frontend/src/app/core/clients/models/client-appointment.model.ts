export interface ClientAppointment {
  id: string;
  startAt: string;
  endAt: string;
  status: string;
  price: number;
  service: {
    id: string;
    name: string;
  };
}

export interface ClientUpcomingAppointment {
  next: ClientAppointment | null;
  last: ClientAppointment | null;
}
