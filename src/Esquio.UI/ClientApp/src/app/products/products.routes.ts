import { RouteConfig } from 'vue-router';

export default (): RouteConfig[] => {
  return [
    {
      path: '/products',
      component: () => import('./Products.vue'),
      children: [
        {
          path: '',
          name: 'products-list',
          component: () => import('./ProductsList.vue')
        },
        {
          path: 'add',
          name: 'products-add',
          component: () => import('./ProductsForm.vue')
        },
        {
          path: ':id',
          name: 'products-edit',
          component: () => import('./ProductsForm.vue'),
          props: true
        },
        {
          path: ':productId/add',
          name: 'flags-add',
          component: () => import('./flags/FlagsForm.vue'),
          props: true
        },
        {
          path: ':productId/:id',
          name: 'flags-edit',
          component: () => import('./flags/FlagsForm.vue'),
          props: true
        },
      ]
    }
  ];
};
