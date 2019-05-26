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
                  path: 'add-feature',
                  name: 'features-add',
                    component: () => import('./FeaturesForm.vue')
                },
            ]
        }
    ];
};
