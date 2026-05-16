export interface Appointment {
  id: string;
  startAt: Date;
  endAt: Date;
  status: string;
  price: number;
  client: {
    id: string;
    name: string;
    email: string;
    phone: string;
  };
  service: {
    id: string;
    name: string;
  };
}
