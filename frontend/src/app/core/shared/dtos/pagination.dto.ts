export interface PaginatedQuery {
  readonly page: number;
  readonly size: number;
}

export interface PaginatedResponse<T> {
  readonly items: T[];
  readonly totalCount: number;
  readonly page: number;
  readonly pageSize: number;
  readonly totalPages: number;
  readonly hasNextPage: boolean;
  readonly hasPreviousPage: boolean;
}
