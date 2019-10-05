import { AbilityBuilder, Ability } from '@casl/ability';
import { UserPermissions } from './user-permissions.model';
import { AbilityAction as Action, AbilitySubject as Subject } from './user-ability.enum';

export function defineAbilitiesFor(userPermissions: UserPermissions) {
  const { rules, can: allow, cannot: forbid } = AbilityBuilder.extract();

  if (userPermissions.isAuthorized) {
    allow(Action.Create, [Subject.Token]);
  }

  if (userPermissions.readPermission) {
    allow(Action.Read, [Subject.Product, Subject.Flag, Subject.Toggle]);
  }

  if (userPermissions.writePermission) {
    allow(Action.Create, [Subject.Product, Subject.Flag, Subject.Toggle]);
    allow(Action.Update, [Subject.Product, Subject.Flag, Subject.Toggle]);
  }

  if (userPermissions.managementPermission) {
    allow([Action.Manage], [Subject.Permission]);
  }

  if (!userPermissions.isAuthorized) {
    forbid(Action.Manage, [Subject.All]);
  }

  return new Ability(rules);
}
