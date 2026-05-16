export interface AppointmentResponse {
  id: string;
  startAt: string;
  endAt: string;
  status: string;
  price: number;
  client: {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    phonePrefix: string;
  };
  service: {
    id: string;
    name: string;
  };
}
