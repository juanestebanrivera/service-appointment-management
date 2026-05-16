import { TestBed } from '@angular/core/testing';

import { AppointmentsApi } from './appointments-api';

describe('AppointmentsApi', () => {
  let service: AppointmentsApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppointmentsApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
