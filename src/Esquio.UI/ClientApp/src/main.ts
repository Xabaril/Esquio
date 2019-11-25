import { Inject } from 'inversify-props';
import 'reflect-metadata';
import VeeValidate from 'vee-validate';
import { Vue } from 'vue-property-decorator';
import { getSettings } from '~/core';
import { IAuthService, ITranslateService } from '~/shared';
import { containerBuilder } from './app/app.container';
import { Filters } from './app/app.filters';
import { router } from './app/app.router';
import App from './app/App.vue';
import { createStore } from './app/store';
import { configurePlugins, vendor } from './app/vendor';
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

    let options = {
      el: '#app',
      router: router(),
      store: createStore(),
      i18n: this.translateService.i18n,
      render: create => create(App)
    };

    return new Vue(options);
  }
}

new AppModule();
