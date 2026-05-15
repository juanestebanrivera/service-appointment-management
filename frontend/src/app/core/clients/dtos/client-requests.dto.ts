export interface CreateClientRequest {
  userId: string;
  firstName: string;
  lastName: string;
  phonePrefix: string;
  phoneNumber: string;
  email: string;
}

export interface ClientUpdateRequest {
  id: string;
  firstName: string;
  lastName: string;
  phonePrefix: string;
  phoneNumber: string;
  email: string;
  isActive: boolean;
}
