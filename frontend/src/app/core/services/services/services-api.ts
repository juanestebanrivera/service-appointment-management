import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL, SERVICE_ENDPOINTS } from '@core/constants';
import { catchError, map, Observable } from 'rxjs';
import {
  PaginatedServicesQuery,
  ServiceCreateRequest,
  ServiceResponse,
  ServiceUpdateRequest,
} from '../dtos';
import { Service } from '../models';
import { mapServiceResponseArrayToModelArray, mapServiceResponseToModel } from '../mappers';
import { PaginatedResponse } from '@core/shared';
import { returnThrowHttpErrorResponse } from '@core/utils/error-handler';

@Injectable({
  providedIn: 'root',
})
export class ServicesApi {
  readonly #http = inject(HttpClient);

  getAll(query: PaginatedServicesQuery): Observable<PaginatedResponse<Service>> {
    const params = new HttpParams({ fromObject: { ...query } });

    return this.#http
      .get<
        PaginatedResponse<ServiceResponse>
      >(`${API_BASE_URL}${SERVICE_ENDPOINTS.GET_ALL}`, { params })
      .pipe(
        map(response => ({
          ...response,
          items: mapServiceResponseArrayToModelArray(response.items),
        })),
      );
  }

  getById(id: string): Observable<Service> {
    return this.#http
      .get<ServiceResponse>(`${API_BASE_URL}${SERVICE_ENDPOINTS.GET_BY_ID(id)}`)
      .pipe(map(mapServiceResponseToModel));
  }

  create(request: ServiceCreateRequest): Observable<void> {
    return this.#http
      .post<void>(`${API_BASE_URL}${SERVICE_ENDPOINTS.CREATE}`, request)
      .pipe(catchError(returnThrowHttpErrorResponse));
  }

  update(request: ServiceUpdateRequest): Observable<void> {
    return this.#http
      .put<void>(`${API_BASE_URL}${SERVICE_ENDPOINTS.UPDATE(request.id)}`, request)
      .pipe(catchError(returnThrowHttpErrorResponse));
  }

  delete(id: string): Observable<void> {
    return this.#http
      .delete<void>(`${API_BASE_URL}${SERVICE_ENDPOINTS.DELETE(id)}`)
      .pipe(catchError(returnThrowHttpErrorResponse));
  }
}
