import { PaginatedResponse, PaginationInfo } from '~/shared';
import { Token } from './token.model';

export interface ITokensService {
  generate(): Promise<Token>;
  get(pagination?: PaginationInfo): Promise<PaginatedResponse<Token[]>>;
  remove(token: Token): Promise<void>;
}
