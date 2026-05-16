import { AppointmentStatus } from '@core/shared';

export interface Appointment {
  id: string;
  startAt: Date;
  endAt: Date;
  status: AppointmentStatus;
  price: number;
  client: {
    id: string;
    name: string;
    email: string;
    phone: string;
  };
  service: {
    id: string;
    name: string;
  };
}
