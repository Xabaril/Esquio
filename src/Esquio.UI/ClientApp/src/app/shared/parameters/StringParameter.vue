<template>
  <div class="string_parameter">
    <input-text
      class="form-group col-md-6"
      v-model="value"
      id="value_name"
      @blur="onBlurInput"
      :label="$t('parameters.string.valueName')"
      validators="required|min:3"
      :help-label="$t('parameters.string.valueHelp')"
    />

  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import { InputText } from '~/shared';

@Component({
  components: {
    InputText
  }
})
export default class extends Vue {
  public name = 'StringParameter';
  public value = null;
  private lastValue: string;

  @Prop({ required: true }) options: any;

  public created(): void {
    this.value = this.options.value;
    this.lastValue = this.value;
  }

  public onBlurInput(value: string): void {
    this.emitChange(value);
  }

  private emitChange(value: string): void {
    if (value === this.lastValue) {
      return;
    }

    this.$emit('change', value);
    this.lastValue = value;
  }
}
</script>
