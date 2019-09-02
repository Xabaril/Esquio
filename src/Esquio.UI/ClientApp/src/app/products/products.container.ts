import { container } from 'inversify-props';
import { IProductsService } from './iproducts.service';
import { ProductsService } from './products.service';
import { IFlagsService, FlagsService } from './flags';
import { ITagsService } from './shared/tags/itags.service';
import { TagsService } from './shared/tags/tags.service';
import { ITogglesService } from './flags/toggles/itoggles.service';
import { TogglesService } from './flags/toggles/toggles.service';

export default () => {
  container.addSingleton<ITagsService>(TagsService);
  container.addSingleton<ITogglesService>(TogglesService);
  container.addSingleton<IProductsService>(ProductsService);
  container.addSingleton<IFlagsService>(FlagsService);
};
