import { UserRole } from '../enums';

export function mapToUserRole(value: string): UserRole {
  const role = Object.values(UserRole).find(r => r === value);

  return role ? role : UserRole.Client;
}
