<template>
<section class="cities container-medium">
  <header class="cities-header text-big">{{'cities.title' | t }}</header>
  <router-view></router-view>
</section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'Cities';
}
</script>

<style lang="scss" scoped>
.cities {
  &-header {
    color: get-color(primary);
  }
}
</style>
