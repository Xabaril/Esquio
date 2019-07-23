<template>
  <section class="toggles_form container u-container-medium pl-0 pr-0">
    <div class="row">
      <h1>{{$t('toggles.detail')}}</h1>
    </div>
    <form class="row">
      <input-text
        class="toggles_form-group form-group col-md-6"
        :class="{'is-disabled': this.isEditing}"
        v-model="form.typeName"
        id="toggle_name"
        :label="$t('toggles.fields.typeName')"
        validators="required|min:5"
        :help-label="$t('toggles.placeholders.typeHelp')"
      />
    </form>

    {{types}}

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
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import {
  Floating,
  FloatingTop,
  FloatingDelete,
  InputText,
  FloatingModifier,
  FloatingContainer
} from '~/shared';
import { AlertType } from '~/core';
import { Toggle } from './toggle.model';
import { ITogglesService } from './itoggles.service';

@Component({
  components: {
    Floating,
    FloatingTop,
    FloatingDelete,
    FloatingContainer,
    InputText
  }
})
export default class extends Vue {
  public name = 'TogglesForm';
  public isLoading = false;
  public types = null;
  public form: Toggle = { id: null, typeName: null, parameters: null };

  @Inject() togglesService: ITogglesService;

  @Prop({ type: [String, Number]}) productId: string;
  @Prop({ type: [String, Number]}) toggleId: string;
  @Prop({ type: [String, Number]}) id: string; // FeatureId

  get isEditing(): boolean {
    return !!this.toggleId;
  }

  get areActionsDisabled(): boolean {
    return (
      !this.form.typeName ||
      this.$validator.errors.count() > 0
    );
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

  private async getTypes(): Promise<void> {
    this.types = await this.togglesService.types();
  }

  private async getToggle(): Promise<void> {
    this.isLoading = true;
    try {
      const { typeName, id, parameters } = await this.togglesService.detail(
        Number(this.toggleId)
      );

      this.form.typeName = typeName;
      this.form.id = id;
      this.form.parameters = parameters;
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
      !(await this.$confirm(
        this.$t('toggles.confirm.title', [this.form.id])
      ))
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
      name: 'flags-edit',
    });
  }
}
</script>

<style lang="scss" scoped>
.toggles_form {
  &-group {
    padding-left: 0;
  }
}
</style>
