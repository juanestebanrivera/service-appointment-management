import { TestBed } from '@angular/core/testing';

import { ClientAppointmentsApi } from './client-appointments-api';

describe('ClientAppointmentsApi', () => {
  let service: ClientAppointmentsApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientAppointmentsApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
