import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import {
  AppointmentResponse,
  CreateAppointmentRequest,
  RescheduleAppointmentRequest,
} from '../dtos';
import { API_BASE_URL, APPOINTMENT_ENDPOINTS } from '@core/constants';
import { mapAppointmentResponseArrayToModelArray, mapAppointmentResponseToModel } from '../mappers';
import { returnThrowHttpErrorResponse } from '@core/utils/error-handler';
import { Appointment } from '../models';

@Injectable({
  providedIn: 'root',
})
export class AppointmentsApi {
  readonly #http = inject(HttpClient);

  getAll(date: Date): Observable<Appointment[]> {
    return this.#http
      .get<
        AppointmentResponse[]
      >(`${API_BASE_URL}${APPOINTMENT_ENDPOINTS.GET_ALL}`, { params: { date: date.toISOString() } })
      .pipe(map(response => mapAppointmentResponseArrayToModelArray(response)));
  }

  getById(id: string): Observable<Appointment> {
    return this.#http
      .get<AppointmentResponse>(`${API_BASE_URL}${APPOINTMENT_ENDPOINTS.GET_BY_ID(id)}`)
      .pipe(map(response => mapAppointmentResponseToModel(response)));
  }

  create(request: CreateAppointmentRequest): Observable<void> {
    return this.#http
      .post<void>(`${API_BASE_URL}${APPOINTMENT_ENDPOINTS.CREATE}`, request)
      .pipe(catchError(returnThrowHttpErrorResponse));
  }

  reschedule(request: RescheduleAppointmentRequest): Observable<void> {
    return this.#http
      .patch<void>(`${API_BASE_URL}${APPOINTMENT_ENDPOINTS.RESCHEDULE(request.id)}`, {
        newStartTime: request.newStartTime.toISOString(),
      })
      .pipe(catchError(returnThrowHttpErrorResponse));
  }

  cancel(id: string): Observable<void> {
    return this.#http
      .post<void>(`${API_BASE_URL}${APPOINTMENT_ENDPOINTS.CANCEL(id)}`, {})
      .pipe(catchError(returnThrowHttpErrorResponse));
  }

  complete(id: string): Observable<void> {
    return this.#http
      .post<void>(`${API_BASE_URL}${APPOINTMENT_ENDPOINTS.COMPLETE(id)}`, {})
      .pipe(catchError(returnThrowHttpErrorResponse));
  }

  confirm(id: string): Observable<void> {
    return this.#http
      .post<void>(`${API_BASE_URL}${APPOINTMENT_ENDPOINTS.CONFIRM(id)}`, {})
      .pipe(catchError(returnThrowHttpErrorResponse));
  }

  noShow(id: string): Observable<void> {
    return this.#http
      .post<void>(`${API_BASE_URL}${APPOINTMENT_ENDPOINTS.NO_SHOW(id)}`, {})
      .pipe(catchError(returnThrowHttpErrorResponse));
  }
}
