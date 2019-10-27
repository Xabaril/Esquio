<template>
  <article class="toggles_form container u-container-medium pl-0 pr-0">
    <header>
      <h1>{{$t('toggles.detail')}}</h1>
    </header>
    <form class="row u-hidden">
      <input-text
        class="toggles_form-group form-group col-md-6 is-disabled"
        :class="{'is-disabled': isEditing}"
        v-model="form.type"
        id="toggle_name"
        :label="$t('toggles.fields.type')"
        validators="required|min:5"
        :help-label="$t('toggles.placeholders.typeHelp')"
      />
    </form>

    <div
      v-if="accordion && !isEditing"
      id="accordion"
      class="toggles_form-accordion"
    >
      <div
        class="toggles_form-card card"
        v-for="(accordionItem, key, i) in accordion"
        :key="key"
      >
        <div class="toggles_form-header card-header">
          <button
              class="btn toggles_form-action-button"
              data-toggle="collapse"
              :data-target="`#c${i}`"
              aria-expanded="true"
            >
              <i class="material-icons">add_circle_outline</i>
          </button>
          <h2 class="toggles_form-title">
          {{key}}
          </h2>
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
                />
                <span class="toggles_form-friendlyname">{{toggleType.friendlyName}}</span>
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
                :title="form.type"
                variant="outline-secondary"
                tag="label"
              >
                {{form.friendlyName || form.type}}
              </b-button>
            </b-button-group>
          </div>
        </div>
      </div>
    </div>

    <div
      v-if="paramDetails"
      class="mt-3 mb-3"
      :class="{'is-disabled': isEditing && isLoading}"
    >
      <div
        v-for="(parameter, key) in paramDetails"
        :key="key"
      >

        <h4>{{parameter.name}}</h4>
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
        v-if="isEditing && $can($constants.AbilityAction.Delete, $constants.AbilitySubject.Toggle)"
        :text="$t('toggles.actions.delete')"
        :disabled="areActionsDisabled"
        @click="onClickDelete"
      />

      <FloatingSave
        v-if="!isEditing"
        :text="$t('toggles.actions.save')"
        :disabled="areActionsDisabled"
        @click="onClickSave"
      />
    </FloatingContainer>
  </article>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import {
  FloatingSave,
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
    FloatingSave,
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
  public form: Toggle = { id: null, type: null, friendlyName: null, parameters: [] };
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
    return !this.form.type || this.$validator.errors.count() > 0;
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
    this.form.type = value;
  }

  public checkButtonActive(value: string): boolean {
    return this.form.type === value;
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

    this.types.toggles.forEach(({ assembly, type, description, friendlyName }) => {
      this.accordion[assembly] = this.accordion[assembly] || [];

      this.accordion[assembly].push({
        type,
        description,
        friendlyName
      });
    });
  }

  private async getFlag(): Promise<void> {
    this.flag = await this.flagsService.detail(Number(this.id));
  }

  private async getToggle(): Promise<void> {
    this.isLoading = true;
    try {
      const { type, id, parameters, friendlyName } = await this.togglesService.detail(
        Number(this.toggleId)
      );

      this.form.type = type;
      this.form.id = Number(this.toggleId);
      this.form.parameters = parameters || [];
      this.form.friendlyName = friendlyName;
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

    try {
      await this.togglesService.addParameter(this.form, parameter.name, value);
      this.isLoading = false;
      this.$alert(this.$t('toggles.success.update'));
    } catch (e) {
      this.$alert(this.$t('toggles.errors.update'), AlertType.Error);
    }
  }

  private goBack(): void {
    this.$router.push({
      name: 'flags-edit'
    });
  }

  @Watch('form.type') onChangeType() {
    this.getToggleParamsInfo();
  }
}
</script>

<style lang="scss" scoped>
.toggles_form {
  &-group {
    padding-left: 0;
  }

  &-action-button {
    border-radius: 100%;
    line-height: 0;

    &:hover {
      background-color: transparent;
      color: get-color(primary, normal);
    }

    .material-icons {
      font-size: get-font-size(xl);
    }
  }

  .btn-group-vertical {
    margin: 0;
  }

  .toggles_form {
    &-button.btn-outline-secondary {
      text-align: left;
      font-size: get-font-size(s);
      border: 0;
      padding: .25rem 0;
      color: get-color(basic, darkest);

      &:active,
      &.is-active,
      &:focus,
      &:hover {
        background-color: transparent;
        border: 0;
        color: get-color(basic, darkest);
        box-shadow: none;
      }

      input[type='radio'] {
        margin-left: 0;
        margin-top: 0;
      }

      &--editing {
        pointer-events: none;
        background-color: transparent;
      }

      &.is-active {
        background-color: transparent;
      }
    }
  }

  &-accordion {
    flex-direction: column;
  }

  &-card {
    box-shadow: none;
    margin-bottom: .5rem;
    background-color: get-color(basic, brighter);
    min-height: 2.5rem;
    border-radius: .25rem;
  }

  &-header {
    padding: .75rem .5rem;
    display: flex;
    align-items: center;
    border-bottom: 0;

    /deep/ button {
      padding: 0;
      margin: 0;
    }
  }

  &-title {
    margin: 0;
    padding-bottom: 0;
    margin-left: .25rem;
    font-size: get-font-size(m);
    line-height: 1.2rem;
  }

  &-body {
    padding: .25rem 1rem .25rem 2.25rem;
  }

  &-friendlyname {
    display: inline-block;
    transform: translateY(-.2rem);
  }
}
</style>
