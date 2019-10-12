import { container } from 'inversify-props';
import { IUsersPermissionsService, UsersPermissionsService } from './shared';

export default () => {
  container.addSingleton<IUsersPermissionsService>(UsersPermissionsService);
};
