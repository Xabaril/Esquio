<template>
  <div class="string_parameter">
    <input-text
      class="form-group col-md-6"
      v-model="value"
      id="toggle_name"
      :label="$t('parameters.string.typeName')"
      validators="required|min:5"
      :help-label="$t('parameters.string.typeHelp')"
    />

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
