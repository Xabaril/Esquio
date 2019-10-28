import { PaginatedResponse, PaginationInfo } from '~/shared';
import { Flag } from './flag.model';

export interface IFlagsService {
  get(productId: number, pagination?: PaginationInfo): Promise<PaginatedResponse<Flag[]>>;
  detail(id: number): Promise<Flag>;
  add(flag: Flag): Promise<void>;
  update(flag: Flag): Promise<void>;
  rollout(flag: Flag): Promise<void>;
  rollback(flag: Flag): Promise<void>;
  remove(flag: Flag): Promise<void>;
}
