import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { addQueryParams, PaginatedResponse, PaginationInfo } from '~/shared';
import { AuditItem } from './audit-item.model';
import { IAuditService } from './iaudit.service';

@injectable()
export class AuditService implements IAuditService {
  public async get(pagination: PaginationInfo): Promise<PaginatedResponse<AuditItem[]>> {
    const params = {
      pageIndex: pagination.pageIndex - 1,
      pageCount: pagination.pageCount
    };

    const response = await fetch(addQueryParams(`${settings.ApiUrl}/audit`, params));

    if (!response.ok) {
      throw new Error('Cannot fetch audit information');
    }

    const result = await response.json();
    result.pageIndex += 1;

    return result;
  }
}
