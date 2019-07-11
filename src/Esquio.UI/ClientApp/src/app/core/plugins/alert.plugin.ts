import { TranslateResult } from 'vue-i18n';

// This plugin is a wrapper of this.$toasted.global to make it easier to the needs of the project
export enum AlertType {
  Error = 'error',
  Success = 'success'
}

export const AlertPlugin = {
  install (Vue) {
    Vue.prototype.$alert = function (text: string | TranslateResult, type: AlertType = AlertType.Success): void {
      this.$toasted.global[type]({ message: text.toString() });
    };
  }
};

declare module 'vue/types/vue' {
  interface Vue {
    $alert: (text: string | TranslateResult, type?: AlertType) => void;
  }
}
