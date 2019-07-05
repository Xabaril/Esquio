import { PaginatedResponse } from '~/core';
import { Flag } from './flag.model';

export interface IFlagsService {
  get(productId): Promise<PaginatedResponse<Flag[]>>;
  detail(id: number): Promise<Flag>;
  add(flag: Flag): Promise<void>;
  update(flag: Flag): Promise<void>;
  remove(flag: Flag): Promise<void>;
}
