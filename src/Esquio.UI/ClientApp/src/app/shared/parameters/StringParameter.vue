<template>
  <div class="string_parameter col-md-6" data-testid="string-parameter">
    <input-text
      class="form-group"
      v-model="value"
      id="value_name"
      @blur="onBlurInput"
      :label="$t('parameters.string.valueName')"
      validators="required|min:1"
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

  @Prop({ required: true }) options: any;

  public created(): void {
    this.value = this.options.value;
  }

  public onBlurInput(value: string): void {
    this.$emit('change', value);
  }
}
</script>
