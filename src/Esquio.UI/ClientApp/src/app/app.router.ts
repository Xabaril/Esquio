import VueRouter, { NavigationGuard } from 'vue-router';
import { Vue } from 'vue-property-decorator';
import { productsModule } from './products';
import { homeModule } from './home';
import { container, cid } from 'inversify-props';
import { IAuthService } from './shared';
import { registerInterceptor } from './core';

Vue.use(VueRouter);

export function router() {
  const authService = container.get<IAuthService>(cid.IAuthService);

  const requireAuth: NavigationGuard = async (to, from, next) => {
    const user = await authService.getUser();
    if (!user) {
      next({ path: '/login' });
      return;
    }

    registerInterceptor();

    next();
  };

  const checkCallback: NavigationGuard = async (to, from, next) => {
    await authService.callback();
    next('/');
  };

  return new VueRouter({
    mode: 'history',
    routes: [
      ...homeModule.routes(requireAuth),
      ...productsModule.routes(requireAuth),
      {
        path: '/callback',
        beforeEnter: checkCallback
      },
      {
        path: '/login',
        component: () => import('./shared/auth/Login.vue'),
      },
      {
        path: '/logout',
        component: () => import('./shared/auth/Logout.vue'),
      },
      {
        path: '/not-found',
        component: () => import('./shared/NotFound.vue'),
      },
      {
        path: '*',
        redirect: '/not-found',
      }
    ]
  });
}
