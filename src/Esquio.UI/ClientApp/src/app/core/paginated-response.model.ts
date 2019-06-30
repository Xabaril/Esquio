export interface PaginatedResponse<T> {
  count: number;
  pageIndex: number;
  result: T;
  total: number;
}
