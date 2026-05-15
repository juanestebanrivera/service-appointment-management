import { PaginatedQuery } from '@core/shared';

export interface PaginatedClientsQuery extends PaginatedQuery {
  search?: string;
  status?: boolean;
}
