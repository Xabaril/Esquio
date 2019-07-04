import { container } from 'inversify-props';
import { IProductsService } from './iproducts.service';
import { ProductsService } from './products.service';

export default () => {
  container.addSingleton<IProductsService>(ProductsService);
};
