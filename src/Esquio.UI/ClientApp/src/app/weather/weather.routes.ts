import { RouteConfig } from 'vue-router';

export default (): RouteConfig[] => {
    return [
        {
            path: '/weather',
            component: () => import('./Weather.vue'),
            children: [
                {
                    path: '',
                    name: 'weather-list',
                    component: () => import('./WeatherList/WeatherList.vue')
                },
                {
                    path: ':id',
                    name: 'weather-detail',
                    component: () => import('./WeatherDetail/WeatherDetail.vue'),
                    props: true
                }
            ]
        }
    ];
};
