<template>
  <section class="toggles_form container u-container-medium pl-0 pr-0">
    <div class="row">
      <h1>{{$t('toggles.detail')}}</h1>
    </div>
    <form class="row u-hidden">
      <input-text
        class="toggles_form-group form-group col-md-6 is-disabled"
        :class="{'is-disabled': this.isEditing}"
        v-model="form.typeName"
        id="toggle_name"
        :label="$t('toggles.fields.typeName')"
        validators="required|min:5"
        :help-label="$t('toggles.placeholders.typeHelp')"
      />
    </form>

    <div
      v-if="accordion && !isEditing"
      id="accordion"
      class="toggles_form-accordion row"
    >
      <div
        class="toggles_form-card card"
        v-for="(accordionItem, key, i) in accordion"
        :key="key"
      >
        <div class="toggles_form-header card-header">
          <h5 class="toggles_form-title mb-0">
            <button
              class="btn btn-link"
              data-toggle="collapse"
              :data-target="`#c${i}`"
              aria-expanded="true"
            >
              {{key}}
            </button>
          </h5>
        </div>

        <div
          :id="`c${i}`"
          class="collapse"
          :class="{'show': showAccordionCollapsed(i)}"
          data-parent="#accordion"
        >
          <div class="toggles_form-body card-body">
            <b-button-group vertical>
              <b-button
                v-for="(toggleType, tkey) in accordionItem"
                class="toggles_form-button"
                :class="{'is-active': checkButtonActive(toggleType.type), 'is-disabled': isToggleUsedInFlag(toggleType)}"
                :key="tkey"
                :title="toggleType.description"
                variant="outline-secondary"
                tag="label"
                @click="onClickButtonType(toggleType.type)"
              >
                <input
                  type="radio"
                  :checked="checkButtonActive(toggleType.type)"
                  :class="{'is-invisible': isToggleUsedInFlag(toggleType)}"
                /> {{toggleType.type}}
              </b-button>
            </b-button-group>
          </div>
        </div>
      </div>
    </div>

    <div
      v-if="accordion && isEditing"
      id="accordion"
      class="toggles_form-accordion row"
    >
      <div class="toggles_form-card card">
        <div
          class="collapse show"
          data-parent="#accordion"
        >
          <div class="toggles_form-body card-body">
            <b-button-group vertical>
              <b-button
                class="toggles_form-button toggles_form-button--editing is-active"
                :title="form.typeName"
                variant="outline-secondary"
                tag="label"
              >
                {{form.typeName}}
              </b-button>
            </b-button-group>
          </div>
        </div>
      </div>
    </div>

    <div
      v-if="paramDetails"
      :class="{'is-disabled': isEditing && isLoading}"
    >
      <div
        v-for="(parameter, key) in paramDetails"
        :key="key"
      >

        <h3>{{parameter.name}}: {{parameter.clrType}}</h3>
        <p>{{parameter.description}}</p>

        <parameter
          :type="parameter.clrType"
          :options="{value: getParameterValue(parameter)}"
          @change="(value) => onChangeParameterValue(parameter, value)"
        />

      </div>
    </div>

    <FloatingContainer>
      <FloatingDelete
        v-if="this.isEditing"
        :text="$t('toggles.actions.delete')"
        :disabled="areActionsDisabled"
        @click="onClickDelete"
      />

      <Floating
        v-if="!this.isEditing"
        :text="$t('toggles.actions.save')"
        :disabled="areActionsDisabled"
        @click="onClickSave"
      />
    </FloatingContainer>
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import {
  Floating,
  FloatingTop,
  FloatingDelete,
  InputText,
  FloatingModifier,
  FloatingContainer,
  Parameter
} from '~/shared';
import { AlertType } from '~/core';
import { Toggle } from './toggle.model';
import { ITogglesService } from './itoggles.service';
import { ToggleParameter } from './toggle-parameter.model';
import { ToggleParameterDetail } from './toggle-parameter-detail.model';
import { ToggleTypesInfo } from './toggle-types.model';
import { IFlagsService } from '../iflags.service';
import { Flag } from '../flag.model';

@Component({
  components: {
    Floating,
    FloatingTop,
    FloatingDelete,
    FloatingContainer,
    InputText,
    Parameter
  }
})
export default class extends Vue {
  public name = 'TogglesForm';
  public isLoading = false;
  public types = null;
  public form: Toggle = { id: null, typeName: null, parameters: [] };
  public accordion: { [key: string]: any } = null;
  public paramDetails: ToggleParameterDetail[] = null;
  public flag: Flag = null;

  @Inject() togglesService: ITogglesService;
  @Inject() flagsService: IFlagsService;

  @Prop({ type: [String, Number] }) productId: string;
  @Prop({ type: [String, Number] }) toggleId: string;
  @Prop({ type: [String, Number] }) id: string; // FlagId

  get isEditing(): boolean {
    return !!this.toggleId;
  }

  get areActionsDisabled(): boolean {
    return !this.form.typeName || this.$validator.errors.count() > 0;
  }

  public async created(): Promise<void> {
    await this.getFlag();
    await this.getTypes();
    if (!this.isEditing) {
      return;
    }

    await this.getToggle();
  }

  public async onClickSave(): Promise<void> {
    if (this.$validator.errors.count() > 1) {
      return;
    }

    if (this.isEditing) {
      return;
    }

    await this.addToggle();

    this.goBack();
  }

  public async onClickDelete(): Promise<void> {
    if (!(await this.deleteToggle())) {
      return;
    }

    this.goBack();
  }

  public onClickButtonType(value: string): void {
    this.form.typeName = value;
  }

  public checkButtonActive(value: string): boolean {
    return this.form.typeName === value;
  }

  public onChangeParameterValue(parameter: ToggleParameterDetail, value): void {
    if (this.isEditing) {
      this.updateAndSaveParameter(parameter, value);
    } else {
      this.updateParameterForm(parameter, value);
    }
  }

  public getParameterValue(parameter: ToggleParameterDetail): any {
    if (!this.form.parameters || this.form.parameters.length < 1) {
      return null;
    }

    const formParameter = this.form.parameters.find(
      x => x.name === parameter.name
    );

    if (!formParameter) {
      return null;
    }

    return formParameter.value;
  }

  public isToggleUsedInFlag(toggleType: ToggleTypesInfo): boolean {
    if (!this.flag || !this.flag.toggles || this.flag.toggles.length < 1) {
      return false;
    }

    return this.flag.toggles.some(toggle => toggle.type === toggleType.type);
  }

  public showAccordionCollapsed(index): boolean {
    return !this.isEditing && index === 0;
  }

  private async getTypes(): Promise<void> {
    this.types = await this.togglesService.types();
    this.generateAccordion();
  }

  private generateAccordion(): void {
    if (!this.types || !this.types.toggles) {
      return;
    }

    this.accordion = this.accordion || {};

    this.types.toggles.forEach(({ assembly, type, description }) => {
      this.accordion[assembly] = this.accordion[assembly] || [];

      this.accordion[assembly].push({
        type,
        description
      });
    });
  }

  private async getFlag(): Promise<void> {
    this.flag = await this.flagsService.detail(Number(this.id));
  }

  private async getToggle(): Promise<void> {
    this.isLoading = true;
    try {
      const { typeName, id, parameters } = await this.togglesService.detail(
        Number(this.toggleId)
      );

      this.form.typeName = typeName;
      this.form.id = Number(this.toggleId);
      this.form.parameters = parameters || [];
    } catch (e) {
      this.$alert(this.$t('toggles.errors.detail'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async getToggleParamsInfo(): Promise<void> {
    this.isLoading = true;

    try {
      this.paramDetails = await this.togglesService.params(this.form);
    } catch (e) {
      this.$alert(this.$t('toggles.errors.detail'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async addToggle(): Promise<void> {
    try {
      await this.togglesService.add(Number(this.id), this.form);

      this.$alert(this.$t('toggles.success.add'));
    } catch (e) {
      this.$alert(this.$t('toggles.errors.add'), AlertType.Error);
    }
  }

  private async deleteToggle(): Promise<boolean> {
    if (
      !(await this.$confirm(this.$t('toggles.confirm.title', [this.form.id])))
    ) {
      return false;
    }

    try {
      await this.togglesService.remove(this.form);
      this.$alert(this.$t('toggles.success.delete'));
      return true;
    } catch (e) {
      this.$alert(this.$t('toggles.errors.delete'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private updateParameterForm(
    parameter: ToggleParameterDetail,
    value: any
  ): void {
    const formParameter = this.form.parameters.find(
      x => x.name === parameter.name
    );

    if (!formParameter) {
      this.form.parameters.push({
        name: parameter.name,
        value: value,
        type: parameter.clrType
      });

      return;
    }

    formParameter.value = value;
  }

  private async updateAndSaveParameter(
    parameter: ToggleParameterDetail,
    value: any
  ): Promise<void> {
    if (this.isLoading) {
      return;
    }

    this.isLoading = true;
    await this.togglesService.addParameter(this.form, parameter.name, value);
    this.isLoading = false;
  }

  private goBack(): void {
    this.$router.push({
      name: 'flags-edit'
    });
  }

  @Watch('form.typeName') onChangeTypeName() {
    this.getToggleParamsInfo();
  }
}
</script>

<style lang="scss" scoped>
.toggles_form {
  &-group {
    padding-left: 0;
  }

  &-button {
    text-align: left;

    &.is-active {
      color: get-color(basic, brightest);
      background-color: get-color(secondary, bright);
    }

    &--editing {
      pointer-events: none;
    }
  }

  &-accordion {
    border: 1px solid get-color(basic, normal);
    flex-direction: column;
  }

  &-card {
    box-shadow: none;
  }

  &-header {
    padding: 0;

    /deep/ button {
      &:hover {
        background-color: get-color(secondary, bright);
        color: get-color(basic, brightest);
      }
    }
  }

  &-title {
    margin-left: 0.5rem;
    margin-top: 0.5rem;
  }

  &-body {
    padding: 0 1.25rem;
  }
}
</style>
