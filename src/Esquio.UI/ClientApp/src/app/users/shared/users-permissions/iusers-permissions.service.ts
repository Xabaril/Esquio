import { PaginatedResponse, PaginationInfo, UserPermissions } from '~/shared';

export interface IUsersPermissionsService {
  get(pagination?: PaginationInfo): Promise<PaginatedResponse<UserPermissions[]>>;
  add(product: UserPermissions): Promise<void>;
  detail(subjectId: string): Promise<UserPermissions>;
  update(product: UserPermissions): Promise<void>;
  remove(product: UserPermissions): Promise<void>;
}
