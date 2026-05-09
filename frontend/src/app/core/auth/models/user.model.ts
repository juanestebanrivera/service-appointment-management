export interface User {
  id: string;
  name: string;
  email: string;
  role: UserRole;
}

export enum UserRole {
  Admin = 'Admin',
  Client = 'Client',
}
