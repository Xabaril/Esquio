<template>
<section class="products container-medium">
  <header class="products-header text-big">{{'products.title' | t }}</header>
  <router-view></router-view>
</section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'Products';
}
</script>

<style lang="scss" scoped>
.products {
  &-header {
    color: get-color(primary);
  }
}
</style>
