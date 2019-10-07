import { User, UserPermissions } from '~/shared/user';
import { Ability } from '@casl/ability';

export interface IAuthService {
  user: User;
  userPermissions: UserPermissions;
  userAbility: Ability;
  init(): void;
  login(): Promise<void | Oidc.User>;
  logout(): Promise<void | Oidc.User>;
  callback(): Promise<void | Oidc.User>;
  silentCallback(): Promise<void | Oidc.User>;
  silent(): Promise<void | Oidc.User>;
  getUser(): Promise<void | Oidc.User>;
  getRolesAndDefinePermissions(): Promise<void>;
}
