import { ITokensService } from './itokens.service';
import { injectable } from 'inversify-props';
import { Token } from './token.model';
import { settings } from '~/core';

@injectable()
export class TokensService implements ITokensService {
  public async generate(): Promise<Token> {
    const response = await fetch(`${settings.ApiUrl}/v1/apikeys`, {
      method: 'POST',
      body: JSON.stringify({
        name: performance.now(),
      })
    });

    if (!response.ok) {
      throw new Error(`Cannot create tokens for your user`);
    }

    return await response.json();
  }

}
