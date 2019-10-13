import { RouteConfig, NavigationGuard } from 'vue-router';
import { AbilityAction, AbilitySubject } from '~/shared';

export default (requireAuth: NavigationGuard): RouteConfig[] => {
  return [
    {
      path: '/users',
      component: () => import('./Users.vue'),
      children: [
        {
          path: '',
          name: 'users-list',
          component: () => import('./UsersList.vue'),
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Manage,
            subject: AbilitySubject.Permission
          }
        },
        {
          path: 'add',
          name: 'users-add',
          component: () => import('./UsersForm.vue'),
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Manage,
            subject: AbilitySubject.Permission
          }
        },
        {
          path: ':subjectId',
          name: 'users-edit',
          component: () => import('./UsersForm.vue'),
          props: true,
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Manage,
            subject: AbilitySubject.Permission
          }
        },
      ]
    }
  ];
};
