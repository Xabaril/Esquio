export interface ApiResponse<T> {
  count: number;
  pageIndex: number;
  result: T;
  total: number;
}
