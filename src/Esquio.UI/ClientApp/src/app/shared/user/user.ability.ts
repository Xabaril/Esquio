import { AbilityBuilder, Ability } from '@casl/ability';
import { UserPermissions } from './user-permissions.model';
import { AbilityAction as Action, AbilitySubject as Subject } from './user-ability.enum';

export function defineAbilitiesFor(permissions: UserPermissions) {
  const { rules, can: allow, cannot: forbid } = AbilityBuilder.extract();

  if (permissions.isAuthorized) {
    allow(Action.Create, [Subject.Token]);
  }

  if (permissions.readPermission || permissions.writePermission || permissions.managementPermission) {
    allow(Action.Read, [Subject.Product, Subject.Flag, Subject.Toggle]);
  }

  if (permissions.writePermission || permissions.managementPermission) {
    allow(Action.Create, [Subject.Product, Subject.Flag, Subject.Toggle]);
    allow(Action.Update, [Subject.Product, Subject.Flag, Subject.Toggle]);
    allow(Action.Delete, [Subject.Product, Subject.Flag, Subject.Toggle]);
  }

  if (permissions.managementPermission) {
    allow([Action.Manage], [Subject.Permission]);
  }

  if (!permissions.isAuthorized) {
    forbid(Action.Manage, [Subject.All]);
  }

  return new Ability(rules);
}
