import { Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import VeeValidate from 'vee-validate';

import { getSettings, registerInterceptor } from '~/core';
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
    registerInterceptor();

    Vue.use(new Filters());
    Vue.use(VeeValidate, { fieldsBagName: 'veeFields' });
    vendor.forEach(library => Vue.use(library));

    this.bootstrap();
  }



  private async bootstrap(): Promise<Vue> {
    await getSettings();

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
