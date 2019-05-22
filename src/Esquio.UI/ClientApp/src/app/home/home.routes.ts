import { RouteConfig } from 'vue-router';

export default (): RouteConfig[] => {
    return [
        {
            path: '/',
            component: () => import('./Home.vue')
        }
    ];
};
