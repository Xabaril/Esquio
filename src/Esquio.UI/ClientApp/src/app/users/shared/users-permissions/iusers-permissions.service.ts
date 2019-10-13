import { PaginatedResponse } from '~/core';
import { UserPermissions } from '~/shared';

export interface IUsersPermissionsService {
  get(): Promise<PaginatedResponse<UserPermissions[]>>;
  add(product: UserPermissions): Promise<void>;
  detail(subjectId: string): Promise<UserPermissions>;
  update(product: UserPermissions): Promise<void>;
  remove(product: UserPermissions): Promise<void>;
}
