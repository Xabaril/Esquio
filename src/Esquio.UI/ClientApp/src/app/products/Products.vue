<template>
<section class="products">
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
