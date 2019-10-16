import { injectable } from 'inversify-props';
import { settings, PaginatedResponse } from '~/core';
import { IUsersPermissionsService } from './iusers-permissions.service';
import { UserPermissions } from '~/shared';

@injectable()
export class UsersPermissionsService implements IUsersPermissionsService {
  public async get(): Promise<PaginatedResponse<UserPermissions[]>> {
    const response = await fetch(`${settings.ApiUrl}/v1/users`);

    if (!response.ok) {
      throw new Error('Cannot fetch users permissions');
    }

    return response.json();
  }

  public async add(userPermissions: UserPermissions): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/users`, {
      method: 'POST',
      body: JSON.stringify({
        subjectId: userPermissions.subjectId,
        read: userPermissions.readPermission,
        write: userPermissions.writePermission,
        manage: userPermissions.managementPermission
      })
    });

    if (!response.ok) {
      throw new Error(`Cannot create permissions from user ${userPermissions.subjectId}`);
    }
  }

  public async detail(subjectId: string): Promise<UserPermissions> {
    const response = await fetch(`${settings.ApiUrl}/v1/users/${subjectId}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch user ${subjectId}`);
    }

    return response.json();
  }

  public async update(userPermissions: UserPermissions): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/users`, {
      method: 'PUT',
      body: JSON.stringify({
        subjectId: userPermissions.subjectId,
        read: userPermissions.readPermission,
        write: userPermissions.writePermission,
        manage: userPermissions.managementPermission
      })
    });

    if (!response.ok) {
      throw new Error(`Cannot update permissions from user ${userPermissions.subjectId}`);
    }
  }

  public async remove(userPermissions: UserPermissions): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/v1/users/${userPermissions.subjectId}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete permissions from user ${userPermissions.subjectId}`);
    }
  }
}

