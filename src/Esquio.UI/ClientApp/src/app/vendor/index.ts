import { Vue } from 'vue-property-decorator';
import * as bootstrap from './bootstrap';
import 'jquery'; // For bootstrap material
import 'bootstrap-material-design';
import Toasted from 'vue-toasted';
import { abilitiesPlugin } from '@casl/vue';
import { Material, ConfirmPlugin, AlertPlugin, ConstantsPlugin } from '~/core';

export const vendor = [
  ...(<any>Object).values(bootstrap),
  Material,
  Toasted,
  ConfirmPlugin,
  AlertPlugin,
  ConstantsPlugin,
  abilitiesPlugin
];

export function configurePlugins() {
  Vue.toasted.register('error', (x) => x.message, {
    type: 'error',
    position: 'top-right',
    duration: 3000,
    action: {
      icon: 'close',
      class: 'toaster-close',
      onClick: (e, toastObject) => {
        toastObject.goAway(0);
      }
    },
  } as any);

  Vue.toasted.register('success', (x) => x.message, {
    type: 'success',
    position: 'top-right',
    duration: 2000,
    action: {
      icon: 'close',
      class: 'toaster-close',
      onClick: (e, toastObject) => {
        toastObject.goAway(0);
      }
    },
  } as any);
}
