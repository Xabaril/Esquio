import { PaginatedResponse, PaginationInfo, UserPermissions } from '~/shared';

export interface IUsersPermissionsService {
  get(pagination?: PaginationInfo): Promise<PaginatedResponse<UserPermissions[]>>;
  add(userPermission: UserPermissions): Promise<void>;
  detail(subjectId: string): Promise<UserPermissions>;
  update(userPermission: UserPermissions): Promise<void>;
  remove(userPermission: UserPermissions): Promise<void>;
}
