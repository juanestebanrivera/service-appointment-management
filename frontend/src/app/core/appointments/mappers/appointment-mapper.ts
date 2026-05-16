import { AppointmentResponse } from '../dtos';
import { Appointment } from '../models/appointment.model';

export const mapAppointmentResponseToModel = (response: AppointmentResponse): Appointment => {
  return {
    id: response.id,
    startAt: new Date(response.startAt),
    endAt: new Date(response.endAt),
    status: response.status,
    price: response.price,
    client: {
      id: response.client.id,
      name: `${response.client.firstName} ${response.client.lastName}`,
      email: response.client.email,
      phone: `${response.client.phonePrefix}${response.client.phone}`,
    },
    service: {
      id: response.service.id,
      name: response.service.name,
    },
  };
};

export const mapAppointmentResponseArrayToModelArray = (
  responses: AppointmentResponse[],
): Appointment[] => {
  return responses.map(mapAppointmentResponseToModel);
};
