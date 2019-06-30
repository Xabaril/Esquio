import { ApiResponse } from '~/core';
import { Product } from './product.model';

export interface IProductsService {
  get(): Promise<ApiResponse<Product[]>>;
  add(product: Product): Promise<void>;
}
