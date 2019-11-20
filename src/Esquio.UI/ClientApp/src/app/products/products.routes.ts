import { NavigationGuard, RouteConfig } from 'vue-router';
import { AbilityAction, AbilitySubject } from '~/shared';

export default (requireAuth: NavigationGuard): RouteConfig[] => {
  return [
    {
      path: '/products',
      component: () => import('./Products.vue'),
      children: [
        {
          path: '',
          name: 'products-list',
          component: () => import('./ProductsList.vue'),
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Read,
            subject: AbilitySubject.Product
          }
        },
        {
          path: 'add',
          name: 'products-add',
          component: () => import('./ProductsForm.vue'),
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Create,
            subject: AbilitySubject.Product
          }
        },
        {
          path: ':productName',
          name: 'products-edit',
          component: () => import('./ProductsForm.vue'),
          props: true,
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Read,
            subject: AbilitySubject.Product
          }
        },
        {
          path: ':productName/add',
          name: 'flags-add',
          component: () => import('./flags/FlagsForm.vue'),
          props: true,
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Create,
            subject: AbilitySubject.Flag
          }
        },
        {
          path: ':productName/:flagName',
          name: 'flags-edit',
          component: () => import('./flags/FlagsForm.vue'),
          props: true,
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Read,
            subject: AbilitySubject.Flag
          }
        },
        {
          path: ':productName/:flagName/add',
          name: 'toggles-add',
          component: () => import('./flags/toggles/TogglesForm.vue'),
          props: true,
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Create,
            subject: AbilitySubject.Toggle
          }
        },
        {
          path: ':productName/:flagName/:type',
          name: 'toggles-edit',
          component: () => import('./flags/toggles/TogglesForm.vue'),
          props: true,
          beforeEnter: requireAuth,
          meta: {
            action: AbilityAction.Read,
            subject: AbilitySubject.Toggle
          }
        }
      ]
    }
  ];
};
