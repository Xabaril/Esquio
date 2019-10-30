import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { addQueryParams, PaginatedResponse, PaginationInfo } from '~/shared';
import { Flag } from './flag.model';
import { IFlagsService } from './iflags.service';

@injectable()
export class FlagsService implements IFlagsService {
  public async get(productId: number, pagination?: PaginationInfo): Promise<PaginatedResponse<Flag[]>> {
    const params = {
      pageIndex: pagination.pageIndex,
      pageCount: pagination.pageCount
    };

    const response = await fetch(addQueryParams(`${settings.ApiUrl}/v1/products/${productId}/flags`, params));

    if (!response.ok) {
      throw new Error('Cannot fetch flags');
    }

    return response.json();
  }

  public async detail(id: number): Promise<Flag> {
    const response = await fetch(`${settings.ApiUrl}/v1/flags/${id}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch flag ${id}`);
    }

    return response.json();
  }

  public async add(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/flags`, {
      method: 'POST',
      body: JSON.stringify(flag)
    });

    if (!response.ok) {
      throw new Error(`Cannot create flag ${flag.name}`);
    }
  }

  public async update(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/flags`, {
      method: 'PUT',
      body: JSON.stringify({
        ...flag,
        flagId: flag.id
      })
    });

    if (!response.ok) {
      throw new Error(`Cannot update flag ${flag.id}`);
    }
  }

  public async rollout(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/flags/${flag.id}/rollout`, {
      method: 'PUT',
      body: JSON.stringify({ featureId: flag.id })
    });

    if (!response.ok) {
      throw new Error(`Cannot update flag ${flag.id}`);
    }
  }

  public async rollback(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/flags/${flag.id}/rollback`, {
      method: 'PUT',
      body: JSON.stringify({ featureId: flag.id })
    });

    if (!response.ok) {
      throw new Error(`Cannot update flag ${flag.id}`);
    }
  }

  public async remove(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/flags/${flag.id}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete flag ${flag.id}`);
    }
  }
}

