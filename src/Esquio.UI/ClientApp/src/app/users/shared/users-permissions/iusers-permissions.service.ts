import { PaginatedResponse } from '~/core';
import { UserPermissions } from '~/shared';

export interface IUsersPermissionsService {
  get(): Promise<PaginatedResponse<UserPermissions[]>>;
  add(product: UserPermissions): Promise<void>;
  update(product: UserPermissions): Promise<void>;
  remove(product: UserPermissions): Promise<void>;
}
