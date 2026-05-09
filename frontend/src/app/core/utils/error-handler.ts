import { HttpErrorResponse } from '@angular/common/http';
import { ProblemDetails } from '@core/models';

export function getErrorMessage(error: HttpErrorResponse): string {
  const problem: ProblemDetails = error.error as ProblemDetails;

  if (problem?.detail) return problem.detail;
  if (problem?.title) return problem.title;

  return 'Ocurrió un error en el servidor';
}
