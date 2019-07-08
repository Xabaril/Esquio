<template>
  <div class="switch">
    <label>
      <input
        type="checkbox"
        v-model="realValue"
      >
      {{label}}
    </label>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'CustomSwitch';

  @Prop({ default: '', type: String }) label: string;
  @Prop({ required: true, type: Boolean }) value: boolean;

  public get realValue(): boolean {
    return this.value;
  }

  public set realValue(value: boolean) {
    this.$emit('input', value);
    this.$emit('change', value);
  }
}
</script>
