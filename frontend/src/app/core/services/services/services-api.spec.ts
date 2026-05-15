import { TestBed } from '@angular/core/testing';

import { ServicesApi } from './services-api';

describe('ServicesApi', () => {
  let service: ServicesApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ServicesApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
