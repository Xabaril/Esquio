<template>
  <div class="semicolon_parameter">
    <vue-tags-input
        ref="input"
        v-model="formParameter"
        :tags="formParameters"
        :placeholder="$t('parameters.semicolon.placeholder')"
        :validation="parametersValidator"
        @before-adding-tag="onAddFormParameter"
        @before-deleting-tag="onRemoveFormParameter"
        @tags-changed="onChangeFormParameters"
      />

    <h4>{{$t('parameters.semicolon.results')}}</h4>
    <div class="semicolon_parameter-preview">
      {{value}}
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import VueTagsInput from '@johmun/vue-tags-input';
import { FormTag } from '~/products/shared/tags';

@Component({
  components: {
    VueTagsInput
  }
})
export default class extends Vue {
  public name = 'PercentageParameter';
  public formParameters: FormTag[] = [];
  public formParameter = '';
  public value = null;
  public parametersValidator = [
    {
      classes: 'no-symbol',
      rule: /^[\w]+$/,
      disableAdd: true
    }
  ];

  @Prop({ required: true }) options: any;

  public created(): void {
    this.value = this.options.value;
    this.formParameters = this.stringToParameters(this.value);
  }

  public async onAddFormParameter({ tag, addTag }): Promise<void> {
    if (!this.isParameterAllowed(tag)) {
      return;
    }

    await this.addFormParameter(tag);
    addTag();
    (this.$refs.input as Vue).$el.querySelector('input').blur();
  }

  public async onRemoveFormParameter({ tag, deleteTag }): Promise<void> {
    await this.removeFormParameter(tag);
    deleteTag();
  }

  public onChangeFormParameters(formParameters: FormTag[]): void {
    this.formParameters = formParameters;
    this.value = this.parametersToString(this.formParameters);
  }

  private isParameterAllowed(parameter: FormTag): boolean {
    if (this.formParameters && this.formParameters.find(x => x.text === parameter.text)) {
      return false;
    }

    if (parameter.tiClasses.includes('no-symbol')) {
      return false;
    }

    return true;
  }

  private addFormParameter(formParameter: FormTag): void {
    this.value += `;${formParameter.text}`;
  }

  private removeFormParameter(formParameter: FormTag): void {
    this.value = this.value.replace(`;${formParameter.text}`, '');
  }

  private parametersToString(formParameters: FormTag[]): string {
    return formParameters.map(x => x.text).join(';');
  }

  private stringToParameters(parameters: string): FormTag[] {
    if (!parameters) {
      return;
    }

    return parameters.split(';').map(x => ({
      text: x,
      tiClasses: ['']
    }));
  }

  @Watch('value')
  onChangeValue(nextValue): void {
    this.$emit('change', nextValue);
  }
}
</script>

<style lang="scss" scoped>
.semicolon_parameter {
  &-preview {
    opacity: .5;
  }
}
</style>
