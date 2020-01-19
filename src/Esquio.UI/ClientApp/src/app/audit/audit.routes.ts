import { NavigationGuard, RouteConfig } from 'vue-router';
import { AbilityAction, AbilitySubject } from '~/shared';

export default (requireAuth: NavigationGuard): RouteConfig[] => {
  return [
    {
      path: '/audit',
      component: () => import('./Audit.vue'),
      children: [
        {
          path: '',
          name: 'audit-list',
          component: () => import('./AuditList.vue'),
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Read,
            subject: AbilitySubject.Permission
          }
        }
      ]
    }
  ];
};
