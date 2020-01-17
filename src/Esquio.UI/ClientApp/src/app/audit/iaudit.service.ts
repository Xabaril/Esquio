import { PaginatedResponse, PaginationInfo } from '~/shared';
import { AuditItem } from './audit-item.model';

export interface IAuditService {
  get(pagination?: PaginationInfo): Promise<PaginatedResponse<AuditItem[]>>;
}
