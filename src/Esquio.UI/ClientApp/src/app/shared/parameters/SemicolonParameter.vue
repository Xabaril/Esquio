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
    const value = nextValue ? nextValue.trim() : [];

    this.$emit('change', value);
  }
}
</script>
