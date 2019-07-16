import { RouteConfig, NavigationGuard } from 'vue-router';

export default (requireAuth: NavigationGuard): RouteConfig[] => {
    return [
        {
            path: '/',
            name: 'home',
            component: () => import('./Home.vue'),
            beforeEnter: requireAuth
        }
    ];
};
