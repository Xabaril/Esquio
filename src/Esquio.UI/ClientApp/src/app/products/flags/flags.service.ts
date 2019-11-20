import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { addQueryParams, PaginatedResponse, PaginationInfo } from '~/shared';
import { Flag } from './flag.model';
import { IFlagsService } from './iflags.service';

@injectable()
export class FlagsService implements IFlagsService {
  public async get(productName: string, pagination?: PaginationInfo): Promise<PaginatedResponse<Flag[]>> {
    const params = {
      pageIndex: pagination.pageIndex - 1,
      pageCount: pagination.pageCount
    };

    const response = await fetch(addQueryParams(`${settings.ApiUrl}/products/${productName}/features`, params));

    if (!response.ok) {
      throw new Error('Cannot fetch features');
    }

    return response.json();
  }

  public async detail(productName: string, name: string): Promise<Flag> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${name}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch feature ${name} from product ${productName}`);
    }

    return response.json();
  }

  public async add(productName: string, flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features`, {
      method: 'POST',
      body: JSON.stringify(flag)
    });

    if (!response.ok) {
      throw new Error(`Cannot create flag ${flag.name} in product ${productName}`);
    }
  }

  public async update(productName: string, flag: Flag, oldFlag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${oldFlag.name}`, {
      method: 'PUT',
      body: JSON.stringify(flag)
    });

    if (!response.ok) {
      throw new Error(`Cannot update flag ${oldFlag.name} from product ${productName}`);
    }
  }

  public async rollout(productName: string, flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${flag.name}/rollout`, {
      method: 'PUT',
      body: JSON.stringify({ featureName: flag.name })
    });

    if (!response.ok) {
      throw new Error(`Cannot update flag ${flag.name} from product ${productName}`);
    }
  }

  public async rollback(productName: string, flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${flag.name}/rollback`, {
      method: 'PUT',
      body: JSON.stringify({ featureName: flag.name })
    });

    if (!response.ok) {
      throw new Error(`Cannot update flag ${flag.name} from product ${productName}`);
    }
  }

  public async remove(productName: string, flag: Flag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${flag.name}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete flag ${flag.name} from product ${productName}`);
    }
  }
}

