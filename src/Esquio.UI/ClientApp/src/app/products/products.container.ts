import { container } from 'inversify-props';
import { IProductsService } from './iproducts.service';
import { ProductsService } from './products.service';
import { IFlagsService, FlagsService } from './flags';

export default () => {
  container.addSingleton<IProductsService>(ProductsService);
  container.addSingleton<IFlagsService>(FlagsService);
};
