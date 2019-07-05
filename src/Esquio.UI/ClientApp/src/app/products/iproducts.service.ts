import { PaginatedResponse } from '~/core';
import { Product } from './product.model';

export interface IProductsService {
  get(): Promise<PaginatedResponse<Product[]>>;
  detail(id: number): Promise<Product>;
  add(product: Product): Promise<void>;
  update(product: Product): Promise<void>;
  remove(product: Product): Promise<void>;
}
