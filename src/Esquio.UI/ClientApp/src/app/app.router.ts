import VueRouter from 'vue-router';
import { Vue } from 'vue-property-decorator';
import { productsModule } from './products';
// import { homeModule } from './home';
import authRoutes from './shared/auth/auth.routes';

Vue.use(VueRouter);

export function router() {
  const requireAuth = authRoutes.requireAuth;

  return new VueRouter({
    mode: 'history',
    routes: [
      // ...homeModule.routes(requireAuth),
      ...productsModule.routes(requireAuth),
      ...authRoutes.routes(),
      {
        path: '/',
        redirect: '/products'
      },
      {
        path: '/not-found',
        component: () => import('./shared/NotFound.vue'),
      },
      {
        path: '/not-allowed',
        component: () => import('./shared/NotAllowed.vue'),
      },
      {
        path: '*',
        redirect: '/not-found',
      }
    ]
  });
}
