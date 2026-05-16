import { mapToAppointmentStatus } from '@core/shared';
import { ClientAppointmentResponse } from '../dtos';
import { ClientAppointment } from '../models';

export const mapClientAppointmentResponseToModel = (
  response: ClientAppointmentResponse,
): ClientAppointment => {
  return {
    id: response.id,
    startAt: response.startAt,
    endAt: response.endAt,
    status: mapToAppointmentStatus(response.status),
    price: response.price,
    service: {
      id: response.service.serviceId,
      name: response.service.serviceName,
    },
  };
};

export const mapClientAppointmentResponseArrayToModelArray = (
  responses: ClientAppointmentResponse[],
): ClientAppointment[] => {
  return responses.map(mapClientAppointmentResponseToModel);
};
