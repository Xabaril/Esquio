import { PaginatedResponse, PaginationInfo } from '~/shared';
import { Flag } from './flag.model';

export interface IFlagsService {
  get(productName: string, pagination?: PaginationInfo): Promise<PaginatedResponse<Flag[]>>;
  detail(productName: string, name: string): Promise<Flag>;
  add(productName: string, flag: Flag): Promise<void>;
  update(productName: string, flag: Flag, oldFlag: Flag): Promise<void>;
  rollout(productName: string, flag: Flag): Promise<void>;
  rollback(productName: string, flag: Flag): Promise<void>;
  remove(productName: string, flag: Flag): Promise<void>;
}
