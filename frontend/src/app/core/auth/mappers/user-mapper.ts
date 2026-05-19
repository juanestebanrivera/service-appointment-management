import { mapToUserRole } from '@core/shared';
import { User } from '../models';
import { UserResponse } from '../dtos';

export const mapUserResponseToUser = (response: UserResponse): User => ({
  id: response.id,
  clientId: response.clientId,
  name: response.name,
  email: response.email,
  role: mapToUserRole(response.role),
  isActive: response.isActive,
});
