import Vue from 'vue';
import Vuex from 'vuex';
import * as navigation from './modules/navigation';

Vue.use(Vuex);

export const createStore = () => {
  return new Vuex.Store({
    modules: {
      navigation
    }
  });
};
