<template>
  <div class="percentage_parameter">
    <input
      type="text"
      v-model="value"
    >

  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'PercentageParameter';
  public value = null;

  @Prop({ required: true }) options: any;

  public created(): void {
    this.value = Number(this.options.value);
  }

  @Watch('value')
  onChangeValue(nextValue, prevValue) {
    const nextValueNumber = Number(nextValue);
    const prevValueNumber = Number(prevValue);

    if (nextValueNumber > 100 || nextValueNumber < 0) {
      this.value = prevValueNumber;
      return;
    }

    if (nextValueNumber === prevValueNumber) {
      return;
    }

    this.$emit('change', this.value);
  }
}
</script>
