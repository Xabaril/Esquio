import { Vue } from 'vue-property-decorator';

import { vendor } from './app/vendor';
import { router } from './app/app.router';
import { containerBuilder } from './app/app.container';
import { Filters } from './app/app.filters';
import store from './app/app.store';
import App from './app/App.vue';

import './styles/app.scss';

export class AppModule {
    constructor() {
        containerBuilder();

        Vue.use(new Filters());
        vendor.forEach(library => Vue.use(library));

        this.bootstrap();
    }



    private async bootstrap(): Promise<Vue> {
        let options = {
            el: '#app',
            router: router(),
            store,
            render: create => create(App)
        };

        return new Vue(options);
    }
}

new AppModule();