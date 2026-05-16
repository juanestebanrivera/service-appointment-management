export interface CreateAppointmentRequest {
  clientId: string;
  serviceId: string;
  startTime: Date;
}

export interface RescheduleAppointmentRequest {
  id: string;
  newStartTime: Date;
}
