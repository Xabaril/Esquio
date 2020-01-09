import { NavigationGuard, RouteConfig } from 'vue-router';
import { AbilityAction, AbilitySubject } from '~/shared';

export default (requireAuth: NavigationGuard): RouteConfig[] => {
  return [
    {
      path: '/tokens',
      component: () => import('./Tokens.vue'),
      children: [
        {
          path: '',
          name: 'tokens-list',
          component: () => import('./TokensList.vue'),
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Read,
            subject: AbilitySubject.Token
          }
        }
      ]
    }
  ];
};
