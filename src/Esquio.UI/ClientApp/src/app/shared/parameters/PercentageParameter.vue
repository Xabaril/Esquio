<template>
  <div class="percentage_parameter">
    <b-progress class="percentage_parameter-bar" :value="value" :max="100" :min="0" show-progress></b-progress>

  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'PercentageParameter';
  public value = 50;

  @Prop({ required: true }) options: {
    [key: string]: any;
  } = null;

  public created(): void {
    this.value = Number(this.options.value);
  }


}
</script>

<style lang="scss">
.percentage_parameter {
  &-bar {
    height: 1rem !important;
  }
}
</style>
