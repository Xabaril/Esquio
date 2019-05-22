<template>
  <div class="app">
    <div class="tabs">
      <router-link class="tab" :to="{ name: 'weather-list'}" active-class="active">{{'common.menu.weather' | t}}</router-link>
      <router-link class="tab" :to="{ name: 'city-list'}" active-class="active">{{'common.menu.cities' | t }}</router-link>
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
