import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL, CLIENT_ENDPOINTS } from '@core/constants';
import { catchError, map, Observable } from 'rxjs';
import { Client } from '../models';
import { ClientResponse, ClientUpdateRequest, CreateClientRequest } from '../dtos';
import { mapClientResponseArrayToModelArray, mapClientResponseToModel } from '../mappers';
import { PaginatedQuery, PaginatedResponse } from '@core/shared';
import { returnThrowHttpErrorResponse } from '@core/utils/error-handler';

@Injectable({
  providedIn: 'root',
})
export class ClientsApi {
  readonly #http = inject(HttpClient);

  getAll(query: PaginatedQuery): Observable<PaginatedResponse<Client>> {
    const params = new HttpParams({ fromObject: { ...query } });

    return this.#http
      .get<
        PaginatedResponse<ClientResponse>
      >(`${API_BASE_URL}${CLIENT_ENDPOINTS.GET_ALL}`, { params })
      .pipe(
        map(response => ({
          ...response,
          items: mapClientResponseArrayToModelArray(response.items),
        })),
      );
  }

  getById(id: string): Observable<Client> {
    return this.#http
      .get<ClientResponse>(`${API_BASE_URL}${CLIENT_ENDPOINTS.GET_BY_ID(id)}`)
      .pipe(map(mapClientResponseToModel));
  }

  create(request: CreateClientRequest): Observable<void> {
    return this.#http
      .post<void>(`${API_BASE_URL}${CLIENT_ENDPOINTS.CREATE}`, request)
      .pipe(catchError(returnThrowHttpErrorResponse));
  }

  update(request: ClientUpdateRequest): Observable<void> {
    return this.#http
      .put<void>(`${API_BASE_URL}${CLIENT_ENDPOINTS.UPDATE(request.id)}`, request)
      .pipe(catchError(returnThrowHttpErrorResponse));
  }

  delete(id: string): Observable<void> {
    return this.#http
      .delete<void>(`${API_BASE_URL}${CLIENT_ENDPOINTS.DELETE(id)}`)
      .pipe(catchError(returnThrowHttpErrorResponse));
  }
}
