import { injectable } from 'inversify-props';
import { settings, PaginatedResponse } from '~/core';
import { IUsersPermissionsService } from './iusers-permissions.service';
import { UserPermissions } from './user-permissions.model';

@injectable()
export class UsersPermissionsService implements IUsersPermissionsService {
  public async get(): Promise<PaginatedResponse<UserPermissions[]>> {
    const response = await fetch(`${settings.ApiUrl}/v1/users/permissions`);

    if (!response.ok) {
      throw new Error('Cannot fetch users permissions');
    }

    return response.json();
  }

  public async add(userPermissions: UserPermissions): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/users/permissions`, {
      method: 'POST',
      body: JSON.stringify(userPermissions)
    });

    if (!response.ok) {
      throw new Error(`Cannot create permissions from user ${userPermissions.id}`);
    }
  }

  public async update(userPermissions: UserPermissions): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/users/permissions`, {
      method: 'PUT',
      body: JSON.stringify(userPermissions)
    });

    if (!response.ok) {
      throw new Error(`Cannot update permissions from user ${userPermissions.id}`);
    }
  }

  public async remove(userPermissions: UserPermissions): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/users/permissions/${userPermissions.id}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete permissions from user ${userPermissions.id}`);
    }
  }
}

