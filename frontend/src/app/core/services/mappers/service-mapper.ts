import { ServiceResponse } from '../dtos';
import { Service } from '../models';

export const mapServiceResponseToModel = (response: ServiceResponse): Service => {
  return {
    id: response.id,
    name: response.name,
    description: response.description,
    price: response.price,
    duration: timeSpanToDate(response.duration),
    isActive: response.isActive,
  };
};

export const mapServiceResponseArrayToModelArray = (responses: ServiceResponse[]): Service[] => {
  return responses.map(mapServiceResponseToModel);
};

const timeSpanToDate = (timeSpan: string): Date => {
  const [hours, minutes, seconds] = timeSpan.split(':').map(Number);
  const date = new Date();

  date.setHours(hours, minutes, seconds);
  return date;
};
