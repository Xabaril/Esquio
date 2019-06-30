<template>
  <div class="input_text">
    <label
      :for="id"
      class="bmd-label-floating"
    >
      {{label}}
    </label>
    <input
      v-validate="validators"
      type="text"
      class="form-control"
      :id="id"
      :name="id"
      :aria-describedby="helper"
      v-model="inputValue"
    >
    <span
      :id="helper"
      class="input_text-text_small form-text text-muted"
    >{{helpLabel}}</span>
    <span
      v-show="errors.has(id)"
      class="input_text-text_small form-text text-danger"
    >{{ errors.first(id) }}</span>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';

@Component({
  inject: ['$validator']
})
export default class extends Vue {
  public name = 'Input-Text';
  public inputValue: string = null;

  @Prop({ required: true }) id: string;
  @Prop({ required: true }) label: string;
  @Prop({ required: true }) helpLabel: string;
  @Prop({ default: '' }) validators: string;
  @Prop() value: any; // v-model

  public get helper(): string {
    return this.id + '_helper';
  }

  public created(): void {
    this.inputValue = this.value;
    console.log(this.validators);
  }

  @Watch('inputValue') onChangeInput() {
    this.$emit('input', this.inputValue);
  }
}
</script>

<style lang="scss" scoped>
.input_text {
  &-text_small {
    font-size: get-font-size(s);
  }
}
</style>
