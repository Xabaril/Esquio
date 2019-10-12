export interface UserPermissions {
  id?: string;
  subjectId?: string;
  isAuthorized: boolean;
  managementPermission: boolean;
  readPermission: boolean;
  writePermission: boolean;
}
