import VueRouter from 'vue-router';
import { Vue } from 'vue-property-decorator';
import { citiesModule } from './cities';
import { weatherModule } from './weather';

Vue.use(VueRouter);

export function router() {
    return new VueRouter({
        mode: 'history',
        routes: [
            {
                path: '/',
                redirect: '/weather',
            },
            ...citiesModule.routes(),
            ...weatherModule.routes()
        ]
    });
}