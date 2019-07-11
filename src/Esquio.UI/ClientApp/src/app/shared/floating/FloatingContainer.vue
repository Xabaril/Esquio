<template>
  <div class="floating-container">
    <slot />
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RouteConfig } from 'vue-router';
import { FloatingIcon } from './floating-icon';
import { FloatingModifier } from './floating-modifier';

@Component
export default class extends Vue {
  public name = 'FloatingContainer';
}
</script>

<style lang="scss" scoped>
.floating-container {
  $margin: 5vh;

  align-items: flex-end;
  bottom: $margin;
  display: flex;
  flex-direction: column;
  position: fixed;
  right: $margin;

  .floating:nth-child(2n) {
    margin-top: .5rem;
  }
}
</style>
