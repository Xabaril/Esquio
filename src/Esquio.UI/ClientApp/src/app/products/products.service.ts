import { injectable } from 'inversify-props';
import { IProductsService } from './iproducts.service';
import { Product } from './product.model';

@injectable()
export class ProductsService implements IProductsService {
  public get(): Product[] {
    throw new Error('Method not implemented.');
  }
}
