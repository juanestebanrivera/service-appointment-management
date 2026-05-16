import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { PaginatedQuery, PaginatedResponse } from '@core/shared';
import { ClientAppointment, ClientUpcomingAppointment } from '../models';
import { ClientAppointmentResponse, ClientUpcomingAppointmentResponse } from '../dtos';
import { API_BASE_URL, CLIENT_ENDPOINTS } from '@core/constants';
import { map, Observable } from 'rxjs';
import {
  mapClientAppointmentResponseArrayToModelArray,
  mapClientAppointmentResponseToModel,
} from '../mappers/client-appointment-mapper';

@Injectable({
  providedIn: 'root',
})
export class ClientAppointmentsApi {
  readonly #http = inject(HttpClient);

  getAll(
    clientId: string,
    pagination: PaginatedQuery,
  ): Observable<PaginatedResponse<ClientAppointment>> {
    const params = new HttpParams({ fromObject: { ...pagination } });

    return this.#http
      .get<
        PaginatedResponse<ClientAppointmentResponse>
      >(`${API_BASE_URL}${CLIENT_ENDPOINTS.APPOINTMENTS.GET_ALL(clientId)}`, { params })
      .pipe(
        map(response => ({
          ...response,
          items: mapClientAppointmentResponseArrayToModelArray(response.items),
        })),
      );
  }

  getUpcoming(
    clientId: string,
    includeLastAppointment: boolean,
  ): Observable<ClientUpcomingAppointment> {
    return this.#http
      .get<ClientUpcomingAppointmentResponse>(
        `${API_BASE_URL}${CLIENT_ENDPOINTS.APPOINTMENTS.UPCOMING(clientId)}`,
        { params: { includeLast: includeLastAppointment } },
      )
      .pipe(
        map(response => ({
          next: response.nextAppointment
            ? mapClientAppointmentResponseToModel(response.nextAppointment)
            : null,
          last: response.lastAppointment
            ? mapClientAppointmentResponseToModel(response.lastAppointment)
            : null,
        })),
      );
  }
}
