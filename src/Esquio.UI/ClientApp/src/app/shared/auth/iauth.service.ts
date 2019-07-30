import { User } from './user.model';

export interface IAuthService {
  user: User;
  init(): void;
  login(): Promise<void | Oidc.User>;
  logout(): Promise<void | Oidc.User>;
  callback(): Promise<void | Oidc.User>;
  silentCallback(): Promise<void | Oidc.User>;
  silent(): Promise<void | Oidc.User>;
  getUser(): Promise<void | Oidc.User>;
}
