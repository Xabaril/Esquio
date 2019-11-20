import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { ITokensService } from './itokens.service';
import { Token } from './token.model';

@injectable()
export class TokensService implements ITokensService {
  public async generate(): Promise<Token> {
    const response = await fetch(`${settings.ApiUrl}/apikeys`, {
      method: 'POST',
      body: JSON.stringify({
        name: 'abc-' + Date.now(),
      })
    });

    if (!response.ok) {
      throw new Error(`Cannot create tokens for your user`);
    }

    return await response.json();
  }

}
