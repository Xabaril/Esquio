import { ActionTree, GetterTree, MutationTree } from 'vuex';

export const namespaced = true;

export enum Type {
  SET_TOGGLE_FRIENDLYNAME = 'SET_TOGGLE_FRIENDLYNAME'
}

export interface State {
  toggleFriendlyName: string;
}

export const state = (): State => ({
  toggleFriendlyName: null
});

export const getters: GetterTree<State, any> = {};

export const actions: ActionTree<State, any> = {
  async setFriendly({ commit }, payload) {
    commit(Type.SET_TOGGLE_FRIENDLYNAME, payload);
  }
};

export const mutations: MutationTree<State> = {
  [Type.SET_TOGGLE_FRIENDLYNAME](state, newName) {
    state.toggleFriendlyName = newName;
  },
};
