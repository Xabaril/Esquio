import * as $ from 'jquery';

export const Material = {
  install(Vue) {
    Vue.mixin({
      mounted() {
        $(this.$el).bootstrapMaterialDesign();
      }
    });
  }
};
