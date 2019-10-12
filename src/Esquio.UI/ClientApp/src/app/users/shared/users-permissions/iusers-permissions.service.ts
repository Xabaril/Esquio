import { PaginatedResponse } from '~/core';
import { UserPermissions } from './user-permissions.model';

export interface IUsersPermissionsService {
  get(): Promise<PaginatedResponse<UserPermissions[]>>;
  add(product: UserPermissions): Promise<void>;
  update(product: UserPermissions): Promise<void>;
  remove(product: UserPermissions): Promise<void>;
}
