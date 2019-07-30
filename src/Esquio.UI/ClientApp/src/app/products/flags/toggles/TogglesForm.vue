<template>
  <section class="toggles_form container u-container-medium pl-0 pr-0">
    <div class="row">
      <h1>{{$t('toggles.detail')}}</h1>
    </div>
    <form class="row">
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
      v-if="accordion"
      id="accordion"
    >
      <div
        class="card"
        v-for="(accordionItem, key, i) in accordion"
        :key="key"
      >
        <div
          class="card-header"
        >
          <h5 class="mb-0">
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
          <div class="card-body">
            <b-button-group vertical>
              <b-button
                class="toggles_form-button"
                :class="{'is-active': checkButtonActive(toggleType.type), 'is-disabled': isEditing}"
                v-for="(toggleType, tkey) in accordionItem"
                :key="tkey"
                :title="toggleType.description"
                variant="outline-secondary"
                tag="label"
                @click="onClickButtonType(toggleType.type)"
              >
                <input v-if="!isEditing" type="radio" :checked="checkButtonActive(toggleType.type)" /> {{toggleType.type}}
              </b-button>
            </b-button-group>
          </div>
        </div>
      </div>
    </div>

    <div v-if="paramDetails">
      <div v-for="(parameter, key) in paramDetails" :key="key">
        {{parameter.clrType}} <br/>
        {{parameter.name}} <br/>
        {{parameter.description}}
        <parameter :type="parameter.clrType" :options="{value: '888'}" @change="(value) => onChangeParameterValue(parameter, value)"/>

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
  public form: Toggle = { id: null, typeName: null, parameters: null };
  public accordion: { [key: string]: any } = null;
  public paramDetails: ToggleParameterDetail[] = null;

  @Inject() togglesService: ITogglesService;

  @Prop({ type: [String, Number] }) productId: string;
  @Prop({ type: [String, Number] }) toggleId: string;
  @Prop({ type: [String, Number] }) id: string; // FeatureId

  get isEditing(): boolean {
    return !!this.toggleId;
  }

  get areActionsDisabled(): boolean {
    return !this.form.typeName || this.$validator.errors.count() > 0;
  }

  public async created(): Promise<void> {
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
    console.log(value, parameter);
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

  private async getToggle(): Promise<void> {
    this.isLoading = true;
    try {
      const { typeName, id, parameters } = await this.togglesService.detail(
        Number(this.toggleId)
      );

      this.form.typeName = typeName;
      this.form.id = Number(this.toggleId);
      this.form.parameters = parameters;
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
      background-color: get-color(secondary);
    }
  }
}
</style>
