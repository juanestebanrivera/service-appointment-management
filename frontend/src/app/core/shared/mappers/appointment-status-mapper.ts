import { AppointmentStatus } from '../enums';

export function mapToAppointmentStatus(value: string): AppointmentStatus {
  const status = Object.values(AppointmentStatus).find(s => s === value);

  return status ? status : AppointmentStatus.Pending;
}
