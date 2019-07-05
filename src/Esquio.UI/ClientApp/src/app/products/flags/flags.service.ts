import { injectable } from 'inversify-props';
import { settings, PaginatedResponse } from '~/core';
import { IFlagsService } from './iflags.service';
import { Flag } from './flag.model';

@injectable()
export class FlagsService implements IFlagsService {
  public async get(productId: number): Promise<PaginatedResponse<Flag[]>> {
    const response = await fetch(`${settings.apiUrl}/v1/products/${productId}/flags`);

    if (!response.ok) {
      throw new Error('Cannot fetch flags');
    }

    return response.json();
  }

  public async detail(id: number): Promise<Flag> {
    const response = await fetch(`${settings.apiUrl}/v1/flags/${id}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch flag ${id}`);
    }

    return response.json();
  }

  public async add(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.apiUrl}/v1/flags`, {
      method: 'POST',
      body: JSON.stringify(flag),
      headers: {
        // 'Authorization': `bearer ${token}`,
        'Content-Type': 'application/json', // TODO: interceptor
      }
    });

    if (!response.ok) {
      throw new Error(`Cannot create flag ${flag.name}`);
    }
  }

  public async update(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.apiUrl}/v1/flags`, {
      method: 'PUT',
      body: JSON.stringify(flag),
      headers: {
        // 'Authorization': `bearer ${token}`,
        'Content-Type': 'application/json', // TODO: interceptor
      }
    });

    if (!response.ok) {
      throw new Error(`Cannot update flag ${flag.id}`);
    }
  }

  public async remove(flag: Flag): Promise<void> {
    const response = await fetch(`${settings.apiUrl}/v1/flags/${flag.id}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete flag ${flag.id}`);
    }
  }
}

