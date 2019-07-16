import { RouteConfig, NavigationGuard } from 'vue-router';
import { container, cid } from 'inversify-props';
import { registerInterceptor } from '~/core';
import { IAuthService } from '.';

const routes = (): RouteConfig[] => {
  return [
    {
      path: '/callback',
      beforeEnter: checkCallback
    },
    {
      path: '/login',
      component: () => import('./Login.vue'),
    },
    {
      path: '/silent',
      component: () => import('./Silent.vue'),
    },
    {
      path: '/logout',
      component: () => import('./Logout.vue'),
    },
  ];
};

const checkCallback: NavigationGuard = async (to, from, next) => {
  const authService = container.get<IAuthService>(cid.IAuthService);
  await authService.callback();
  next('/');
};

const requireAuth: NavigationGuard = async (to, from, next) => {
  const authService = container.get<IAuthService>(cid.IAuthService);
  const user = await authService.getUser();
  if (!user) {
    next({ path: '/login' });
    return;
  }

  registerInterceptor();

  next();
};

export default {
  routes,
  checkCallback,
  requireAuth
};
