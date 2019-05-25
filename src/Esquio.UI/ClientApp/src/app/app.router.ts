import VueRouter from 'vue-router';
import { Vue } from 'vue-property-decorator';
import { productsModule } from './products';
import { homeModule } from './home';

Vue.use(VueRouter);

export function router() {
  return new VueRouter({
    mode: 'history',
    routes: [
      ...homeModule.routes(),
      ...productsModule.routes(),
      {
        path: '*',
        redirect: '/'
      }
    ]
  });
}
