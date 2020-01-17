import { container } from 'inversify-props';
import { auditModule } from '~/audit';
import { homeModule } from '~/home';
import { productsModule } from '~/products';
import * as s from '~/shared';
import { tokensModule } from '~/tokens';
import { usersModule } from '~/users';

// How to inject a dependency
// @Inject() nameService: INameService;

export function containerBuilder(): void {

    // Bind shared services
    container.addSingleton<s.ITranslateService>(s.TranslateService);
    container.addSingleton<s.IAuthService>(s.AuthService);
    container.addSingleton<s.IDateService>(s.DateService);

    // Bind services for each module
    usersModule.container();
    tokensModule.container();
    auditModule.container();
    productsModule.container();
    homeModule.container();
}
