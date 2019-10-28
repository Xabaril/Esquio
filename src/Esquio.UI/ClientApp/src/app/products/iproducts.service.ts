import { PaginatedResponse, PaginationInfo } from '~/shared';
import { Product } from './product.model';

export interface IProductsService {
  get(pagination?: PaginationInfo): Promise<PaginatedResponse<Product[]>>;
  detail(id: number): Promise<Product>;
  add(product: Product): Promise<void>;
  update(product: Product): Promise<void>;
  remove(product: Product): Promise<void>;
}
