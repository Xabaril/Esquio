<template>
  <div class="app">
    <div class="tabs">
      <router-link class="tab" :to="{ name: 'home'}" active-class="active">{{'common.menu.home' | t}}</router-link>
      <router-link class="tab" :to="{ name: 'products-list'}" active-class="active">{{'common.menu.products' | t }}</router-link>
    </div>
    <router-view class="section"></router-view>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'App';
}
</script>

<style lang="scss" scoped>
.app {
  color: get-color(basic, brighter);
}
</style>
