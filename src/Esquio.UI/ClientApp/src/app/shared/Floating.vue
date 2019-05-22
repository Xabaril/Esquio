<template>
  <div class="floating">
    <div class="btn-float-dropdown dropup">
      <button
        aria-expanded="false"
        aria-haspopup="true"
        class="btn btn-float btn-primary"
        data-toggle="dropdown"
        type="button"
      ><i class="material-icons">add</i></button>
      <div class="dropdown-menu">
        <button
          class="btn btn-float btn-light btn-sm"
          type="button"
        ><i class="material-icons">link</i></button>
        <button
          class="btn btn-float btn-light btn-sm"
          type="button"
        ><i class="material-icons">link</i></button>
        <button
          class="btn btn-float btn-light btn-sm"
          type="button"
        ><i class="material-icons">link</i></button>
      </div>
    </div>

    <b-dropdown class="btn-float-dropdown dropup btn btn-float btn-primary">
      <b-dropdown-item class="btn btn-float btn-primary"><i class="material-icons">link</i></b-dropdown-item>
      <b-dropdown-item class="btn btn-float btn-primary"><i class="material-icons">link</i></b-dropdown-item>
      <b-dropdown-item class="btn btn-float btn-primary"><i class="material-icons">link</i></b-dropdown-item>
    </b-dropdown>
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
