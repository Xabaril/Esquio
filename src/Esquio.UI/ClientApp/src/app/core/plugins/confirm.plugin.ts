import { TranslateResult } from 'vue-i18n';

// This plugin is a wrapper of $bvModal.msgBoxConfirm to make it easier to the needs of the project
export const ConfirmPlugin = {
  install (Vue) {
    Vue.prototype.$confirm = function (text: string | TranslateResult): Promise<boolean> {
      console.log(text);
      return this.$bvModal.msgBoxConfirm(text.toString());
    };
  }
};

declare module 'vue/types/vue' {
  interface Vue {
    $confirm: (text: string | TranslateResult) => Promise<boolean>;
  }
}
