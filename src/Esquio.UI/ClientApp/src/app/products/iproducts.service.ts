import { Product } from './product.model';

export interface IProductsService {
  get(): Product[];
}
