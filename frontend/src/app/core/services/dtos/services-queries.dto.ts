import { PaginatedQuery } from '@core/shared';

export interface PaginatedServicesQuery extends PaginatedQuery {
  search?: string;
  status?: boolean;
}
