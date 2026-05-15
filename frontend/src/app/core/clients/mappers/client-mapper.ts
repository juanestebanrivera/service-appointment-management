import { ClientResponse } from '../dtos';
import { Client } from '../models';

export const mapClientResponseToModel = (response: ClientResponse): Client => {
  return {
    id: response.id,
    firstName: response.firstName,
    lastName: response.lastName,
    fullName: `${response.firstName} ${response.lastName}`,
    phone: response.phoneNumber,
    phonePrefix: response.phonePrefix,
    email: response.email,
    isActive: response.isActive,
  };
};

export const mapClientResponseArrayToModelArray = (responses: ClientResponse[]): Client[] => {
  return responses.map(mapClientResponseToModel);
};
