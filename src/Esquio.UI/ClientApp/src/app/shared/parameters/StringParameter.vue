<template>
  <div class="string_parameter">
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
  public name = 'StringParameter';
  public value = null;

  @Prop({ required: true }) options: any;

  public created(): void {
    this.value = this.options.value;
  }

  @Watch('value')
  onChangeValue(nextValue, prevValue) {
    this.$emit('change', this.value);
  }
}
</script>
