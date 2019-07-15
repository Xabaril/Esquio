import 'reflect-metadata';
import { Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import VeeValidate from 'vee-validate';

import { getSettings, registerInterceptor } from '~/core';
import { vendor, configurePlugins } from './app/vendor';
import { router } from './app/app.router';
import { containerBuilder } from './app/app.container';
import { Filters } from './app/app.filters';
import store from './app/app.store';
import App from './app/App.vue';

import { ITranslateService, IAuthService } from '~/shared';
import './styles/app.scss';

export class AppModule {
  @Inject() translateService: ITranslateService;
  @Inject() authService: IAuthService;

  constructor() {
    containerBuilder();

    Vue.use(new Filters());
    Vue.use(VeeValidate, { fieldsBagName: 'veeFields' });
    vendor.forEach(library => Vue.use(library));
    configurePlugins();

    this.bootstrap();
  }



  private async bootstrap(): Promise<Vue> {
    await getSettings();
    this.authService.init();
    registerInterceptor();

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
