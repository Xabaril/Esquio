<template>
  <div class="string_parameter">
    <input-text
      class="form-group col-md-6"
      v-model="value"
      id="value_name"
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
