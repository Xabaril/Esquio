import { container } from 'inversify-props';
import * as s from '~/shared';
import { homeModule } from '~/home';
import { productsModule } from '~/products';

// How to inject a dependency
// @Inject() nameService: INameService;

export function containerBuilder(): void {

    // Bind shared services
    container.addSingleton<s.ITranslateService>(s.TranslateService);
    container.addSingleton<s.IAuthService>(s.AuthService);

    // Bind services for each module
    productsModule.container();
    homeModule.container();
}
