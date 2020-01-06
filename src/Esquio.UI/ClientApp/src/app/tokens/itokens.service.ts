import { PaginatedResponse, PaginationInfo } from '~/shared';
import { Token } from './token.model';

export interface ITokensService {
  add(token: Token): Promise<Token>;
  get(pagination?: PaginationInfo): Promise<PaginatedResponse<Token[]>>;
  remove(token: Token): Promise<void>;
}
