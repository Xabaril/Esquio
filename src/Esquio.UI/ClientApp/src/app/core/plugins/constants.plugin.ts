import { AbilityAction, AbilitySubject } from '~/shared';

const constants = {
  AbilityAction: AbilityAction,
  AbilitySubject: AbilitySubject
};

export const ConstantsPlugin = {
  install (Vue) {
    Vue.prototype.$constants = constants;
  }
};

declare module 'vue/types/vue' {
  interface Vue {
    $constants: typeof constants;
  }
}
