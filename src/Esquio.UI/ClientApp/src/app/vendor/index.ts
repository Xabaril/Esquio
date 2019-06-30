import { Vue } from 'vue-property-decorator';
import * as bootstrap from './bootstrap';
import 'jquery'; // For bootstrap material
import 'bootstrap-material-design';
import Toasted from 'vue-toasted';
import { Material } from '~/core';

export const vendor = [
  ...(<any>Object).values(bootstrap),
  Material,
  Toasted
];

export function configurePlugins() {
  Vue.toasted.register('apierror', (x) => x.message, {
    type: 'error',
    position: 'top-right',
    icon: 'error_outline',
    action: {
      icon: 'close',
      class: 'toaster-close',
      onClick: (e, toastObject) => {
        toastObject.goAway(0);
      }
    },
  } as any);
}
