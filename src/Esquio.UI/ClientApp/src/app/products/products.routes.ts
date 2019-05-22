import { RouteConfig } from 'vue-router';

export default (): RouteConfig[] => {
    return [
        {
            path: '/products',
            component: () => import('./Products.vue'),
            children: [
                // {
                //     path: '',
                //     name: 'products-list',
                //     component: () => import('./ProductsList/ProductsList.vue')
                // },
                // {
                //     path: ':id',
                //     name: 'products-detail',
                //     component: () => import('./ProductsDetail/ProductsDetail.vue'),
                //     props: true
                // }
            ]
        }
    ];
};
