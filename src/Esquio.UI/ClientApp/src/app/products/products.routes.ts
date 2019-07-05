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
                  path: 'edit',
                  redirect: '/products'
                },
                {
                  path: 'edit/:id',
                  name: 'products-edit',
                    component: () => import('./ProductsForm.vue'),
                    props: true
                },
                {
                  path: ':productId/add-flag',
                  name: 'flags-add',
                    component: () => import('./flags/FlagsForm.vue')
                },
                {
                  path: ':productId/edit-flag',
                  redirect: ':productId/add-flag'
                },
                {
                  path: ':productId/edit-flag/:id',
                  name: 'flags-edit',
                    component: () => import('./flags/FlagsForm.vue')
                },
            ]
        }
    ];
};
