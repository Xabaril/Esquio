<template>
  <section class="home container-medium">
    <header class="home-header text-big">{{$t('home.title')}}</header>
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'Home';
}
</script>

<style lang="scss" scoped>
.home {
  &-header {
    color: get-color(primary);
  }
}
</style>
