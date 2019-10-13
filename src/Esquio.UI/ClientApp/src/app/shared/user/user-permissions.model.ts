export interface UserPermissions {
  subjectId?: string;
  isAuthorized?: boolean;
  managementPermission: boolean;
  readPermission: boolean;
  writePermission: boolean;
}
