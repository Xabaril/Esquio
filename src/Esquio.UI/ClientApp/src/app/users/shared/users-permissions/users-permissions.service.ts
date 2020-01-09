import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { addQueryParams, PaginatedResponse, PaginationInfo, UserPermissions } from '~/shared';
import { IUsersPermissionsService } from './iusers-permissions.service';

@injectable()
export class UsersPermissionsService implements IUsersPermissionsService {
  public async get(pagination: PaginationInfo): Promise<PaginatedResponse<UserPermissions[]>> {
    const params = {
      pageIndex: pagination.pageIndex - 1,
      pageCount: pagination.pageCount
    };

    const response = await fetch(addQueryParams(`${settings.ApiUrl}/users`, params));

    if (!response.ok) {
      throw new Error('Cannot fetch users permissions');
    }

    const result = await response.json();
    result.PaginatedResponse += 1;

    return result;
  }

  public async add(userPermissions: UserPermissions): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/users`, {
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
    const response = await fetch(`${settings.ApiUrl}/users/${subjectId}`);

    if (!response.ok) {
      throw new Error(`Cannot fetch user ${subjectId}`);
    }

    return response.json();
  }

  public async update(userPermissions: UserPermissions): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/users`, {
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
    const response = await fetch(`${settings.ApiUrl}/users/${userPermissions.subjectId}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete permissions from user ${userPermissions.subjectId}`);
    }
  }
}

