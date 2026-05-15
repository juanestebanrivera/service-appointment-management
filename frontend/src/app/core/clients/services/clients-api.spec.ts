import { TestBed } from '@angular/core/testing';

import { ClientsApi } from './clients-api';

describe('ClientsApi', () => {
  let service: ClientsApi;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientsApi);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
