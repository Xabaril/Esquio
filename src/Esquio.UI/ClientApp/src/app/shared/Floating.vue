<template>
  <div class="floating">
    <button type="button" class="btn btn-primary bmd-btn-fab">
  <i class="material-icons">grade</i>
</button>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";

@Component
export default class extends Vue {
  public name = "Floating";
}
</script>

<style lang="scss" scoped>
.floating {
  $margin: 1vw;

  position: fixed;
  bottom: $margin;
  right: $margin;
}
</style>
