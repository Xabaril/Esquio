import { RouteConfig } from 'vue-router';

export default (): RouteConfig[] => {
    return [
        {
            path: '/cities',
            component: () => import('./Cities.vue'),
            children: [
                {
                    path: '',
                    name: 'city-list',
                    component: () => import('./CityList/CityList.vue')
                },
                {
                    path: ':id',
                    name: 'city-detail',
                    component: () => import('./CityDetail/CityDetail.vue'),
                    props: true
                }
            ]
        }
    ];
};
