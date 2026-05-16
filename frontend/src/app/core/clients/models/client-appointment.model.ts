import { AppointmentStatus } from '@core/shared';

export interface ClientAppointment {
  id: string;
  startAt: string;
  endAt: string;
  status: AppointmentStatus;
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
