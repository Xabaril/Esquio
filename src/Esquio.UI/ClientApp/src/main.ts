import { Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';

import { Material } from '~/core/material';
import { vendor } from './app/vendor';
import { router } from './app/app.router';
import { containerBuilder } from './app/app.container';
import { Filters } from './app/app.filters';
import store from './app/app.store';
import App from './app/App.vue';

import './styles/app.scss';
import { ITranslateService } from '~/shared';

export class AppModule {
  @Inject() translateService: ITranslateService;

  constructor() {
    containerBuilder();

    Vue.use(new Filters());
    Vue.use(Material);
    vendor.forEach(library => Vue.use(library));

    this.bootstrap();
  }



  private async bootstrap(): Promise<Vue> {
    let options = {
      el: '#app',
      router: router(),
      store,
      i18n: this.translateService.i18n,
      render: create => create(App)
    };

    return new Vue(options);
  }
}

new AppModule();
