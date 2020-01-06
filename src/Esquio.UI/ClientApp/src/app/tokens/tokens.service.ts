import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { addQueryParams, PaginatedResponse, PaginationInfo } from '~/shared';
import { ITokensService } from './itokens.service';
import { Token } from './token.model';

@injectable()
export class TokensService implements ITokensService {
  public async get(pagination: PaginationInfo): Promise<PaginatedResponse<Token[]>> {
    const params = {
      pageIndex: pagination.pageIndex - 1,
      pageCount: pagination.pageCount
    };

    const response = await fetch(addQueryParams(`${settings.ApiUrl}/apikeys`, params));

    if (!response.ok) {
      throw new Error('Cannot fetch tokens');
    }

    return response.json();
  }

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

  public async remove(token: Token): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/apikeys/${token.name}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete token ${token.name}`);
    }
  }

}
