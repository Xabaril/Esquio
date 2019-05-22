<template>
  <div class="floating">
    <button
      type="button"
      class="floating-button btn btn-primary bmd-btn-fab"
       title="Example text"
    >
      <i class="material-icons">add</i>
      <span class="floating-text is-hidden@s">Example text</span>
    </button>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'Floating';
}
</script>

<style lang="scss" scoped>
.floating {
  $margin: 5vh;

  position: fixed;
  bottom: $margin;
  right: $margin;

  &-button {
    @media screen and (min-width: get-media(s)) {
      align-items: center;
      display: flex;
      border-radius: 2rem !important;
      font-size: get-font-size(m) !important;
      justify-content: space-between;
      padding: 1rem !important;
      width: auto !important;
    }
  }

  &-text {
    display: inline-flex;
    padding-left: .5rem;
  }
}
</style>
