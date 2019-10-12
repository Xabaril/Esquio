import { container } from 'inversify-props';
import * as s from '~/shared';
import { homeModule } from '~/home';
import { productsModule } from '~/products';
import { usersModule } from '~/users';

// How to inject a dependency
// @Inject() nameService: INameService;

export function containerBuilder(): void {

    // Bind shared services
    container.addSingleton<s.ITranslateService>(s.TranslateService);
    container.addSingleton<s.IAuthService>(s.AuthService);
    container.addSingleton<s.ITokensService>(s.TokensService);

    // Bind services for each module
    usersModule.container();
    productsModule.container();
    homeModule.container();
}
