import { Token } from './token.model';

export interface ITokensService {
  generate(): Promise<Token>;
}
