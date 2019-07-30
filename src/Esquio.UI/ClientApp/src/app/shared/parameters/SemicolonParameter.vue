<template>
  <div class="semicolon_parameter">
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
    this.value = this.options.value;
  }

  @Watch('value')
  onChangeValue(nextValue, prevValue) {
    // Temporal
    const value = nextValue ? nextValue.trim().replace(/,/g, ';') : [];

    this.$emit('change', value);
  }
}
</script>
