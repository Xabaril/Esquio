import { container } from 'inversify-props';
import { ITokensService } from './itokens.service';
import { TokensService } from './tokens.service';

export default () => {
  container.addSingleton<ITokensService>(TokensService);
};
