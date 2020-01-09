import { Ability, AbilityBuilder } from '@casl/ability';
import { AbilityAction as Action, AbilitySubject as Subject } from './user-ability.enum';
import { UserPermissions } from './user-permissions.model';

export function defineAbilitiesFor(permissions: UserPermissions) {
  const { rules, can: allow, cannot: forbid } = AbilityBuilder.extract();

  if (permissions.isAuthorized) {
    allow(Action.Create, [Subject.Token]);
  }

  if (permissions.readPermission || permissions.writePermission || permissions.managementPermission) {
    allow(Action.Read, [Subject.Product, Subject.Flag, Subject.Toggle, Subject.Token]);
  }

  if (permissions.writePermission || permissions.managementPermission) {
    allow(Action.Create, [Subject.Product, Subject.Flag, Subject.Toggle, Subject.Token]);
    allow(Action.Update, [Subject.Product, Subject.Flag, Subject.Toggle, Subject.Token]);
    allow(Action.Delete, [Subject.Product, Subject.Flag, Subject.Toggle, Subject.Token]);
  }

  if (permissions.managementPermission) {
    allow([Action.Manage], [Subject.Permission]);
  }

  if (!permissions.isAuthorized) {
    forbid(Action.Manage, [Subject.All]);
  }

  return new Ability(rules);
}
