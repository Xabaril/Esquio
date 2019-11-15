import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { addQueryParams, PaginatedResponse, PaginationInfo } from '~/shared';
import { Flag } from './flag.model';
import { IFlagsService } from './iflags.service';

@injectable()
export class FlagsService implements IFlagsService {
  public async get(productId: number, pagination?: PaginationInfo): Promise<PaginatedResponse<Flag[]>> {
    // TODO: Change productId for productName
    const params = {
      pageIndex: pagination.pageIndex,
      pageCount: pagination.pageCount
    };

    const response = await fetch(addQueryParams(`${settings.ApiUrl}/products/${productId}/features`, params));

    if (!response.ok) {
      throw new Error('Cannot fetch features');
    }

    return response.json();
  }

  public async detail(id: number): Promise<Flag> {
    // TODO: use name instead of id
    const name = '';
    const productName = '';
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${name}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch feature ${id}`);
    }

    return response.json();
  }

  public async add(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/{productname}/features`, {
      method: 'POST',
      body: JSON.stringify(flag)
    });

    if (!response.ok) {
      throw new Error(`Cannot create flag ${flag.name}`);
    }
  }

  public async update(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/{productname}/features/{oldname}`, {
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
    const response = await fetch(`${settings.ApiUrl}/products/{productname}/features/name---->${flag.id}/rollout`, {
      method: 'PUT',
      body: JSON.stringify({ featureId: flag.id })
    });

    if (!response.ok) {
      throw new Error(`Cannot update flag ${flag.id}`);
    }
  }

  public async rollback(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/{productname}/features/name---->${flag.id}/rollback`, {
      method: 'PUT',
      body: JSON.stringify({ featureId: flag.id })
    });

    if (!response.ok) {
      throw new Error(`Cannot update flag ${flag.id}`);
    }
  }

  public async remove(flag: Flag): Promise<void> {
    // TODO: use name instead of id
    const response = await fetch(`${settings.ApiUrl}/products/{productName}/features/name--->${flag.id}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete flag ${flag.id}`);
    }
  }
}

