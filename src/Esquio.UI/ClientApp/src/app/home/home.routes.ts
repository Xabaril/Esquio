import { RouteConfig } from 'vue-router';

export default (): RouteConfig[] => {
    return [
        {
            path: '/',
            name: 'home',
            component: () => import('./Home.vue')
        }
    ];
};
