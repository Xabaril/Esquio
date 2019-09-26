<template>
  <section class="flags_form container u-container-medium">
    <div class="row">
      <h1 class="col col-auto pl-0">{{$t('flags.detail')}}</h1>
      <div class="flags_form-switch col col-auto pl-0">
        <custom-switch v-if="false" v-model="form.enabled" />
      </div>
    </div>
    <form class="row">
      <input-text
        class="flags_form-group form-group col-md-6"
        :class="{'is-disabled': isEditing}"
        v-model="form.name"
        id="flag_name"
        :label="$t('flags.fields.name')"
        validators="required|min:5"
        :help-label="$t('flags.placeholders.nameHelp')"
      />

      <input-text
        class="flags_form-group form-group col-md-6"
        :class="{'is-disabled': isEditing}"
        v-model="form.description"
        id="flag_description"
        :label="$t('flags.fields.description')"
        validators="required|min:5"
        :help-label="$t('flags.placeholders.descriptionHelp')"
      />
    </form>

    <div
      v-if="isEditing"
      class="row"
    >
      <h2>{{$t('tags.title')}}</h2>
    </div>

    <div
      v-if="isEditing"
      class="row"
      :class="{'is-disabled': isLoading}"
    >
      <vue-tags-input
        v-model="formTag"
        :tags="formTags"
        :placeholder="$t('flags.placeholders.tag')"
        @before-adding-tag="onAddFormTag"
        @before-deleting-tag="onRemoveFormTag"
        @tags-changed="onChangeFormTags"
      />
    </div>

    <div
      class="row mt-4"
      v-if="isEditing"
    >
      <h2>{{$t('toggles.title')}}</h2>
      <TogglesList :flagId="id" :toggles="form.toggles" />
    </div>

    <FloatingContainer>
      <FloatingDelete
        v-if="this.isEditing"
        :text="$t('flags.actions.delete')"
        :disabled="areActionsDisabled"
        @click="onClickDelete"
      />

      <FloatingSave
        :text="$t('flags.actions.save')"
        :disabled="areActionsDisabled"
        @click="onClickSave"
      />
    </FloatingContainer>

    <FloatingTop
      v-if="isEditing"
      :text="$t('flags.actions.add_toggle')"
      :to="{name: 'toggles-add', params: { productId: productId, flagId: id }}"
    />
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import VueTagsInput from '@johmun/vue-tags-input';
import { Inject } from 'inversify-props';
import { AlertType } from '~/core';
import {
  FloatingSave,
  FloatingDelete,
  FloatingTop,
  FloatingContainer,
  InputText,
  CustomSwitch,
} from '~/shared';
import { ITagsService, Tag, FormTag } from '~/products/shared/tags';
import { Flag } from './flag.model';
import { IFlagsService } from './iflags.service';
import { TogglesList } from './toggles';

@Component({
  components: {
    FloatingSave,
    FloatingContainer,
    FloatingTop,
    FloatingDelete,
    InputText,
    CustomSwitch,
    VueTagsInput,
    TogglesList
  }
})
export default class extends Vue {
  public name = 'FlagsForm';
  public isLoading = false;
  public tags: Tag[] = null;
  public formTags: FormTag[] = [];
  public formTag = '';
  public isInvalid = false;
  public form: Flag = {
    productId: null,
    id: null,
    name: null,
    description: null,
    enabled: false,
    toggles: null
  };

  public tagsValidator = [
    {
      classes: 'no-symbol',
      rule: /^[\w]+$/,
      disableAdd: true
    }
  ];

  @Inject() flagsService: IFlagsService;
  @Inject() tagsService: ITagsService;

  @Prop({ type: [String, Number] }) id: string;
  @Prop({ type: [String, Number], required: true }) productId: string;

  get isEditing(): boolean {
    return !!this.id;
  }

  get areActionsDisabled(): boolean {
    return (
      !this.form.name ||
      !this.form.description ||
      this.$validator.errors.count() > 0
    );
  }

  public async created(): Promise<void> {
    if (!this.isEditing) {
      return;
    }

    this.isLoading = true;
    await this.getFlag();
    await this.getTags();
    this.isLoading = false;
  }

  public async onAddFormTag({ tag, addTag }): Promise<void> {
    if (!this.isTagAllowed(tag)) {
      return;
    }

    this.isLoading = true;
    await this.addFormTag(tag);
    addTag();
    this.isLoading = false;
  }

  public async onRemoveFormTag({ tag, deleteTag }): Promise<void> {
    this.isLoading = true;
    await this.removeFormTag(tag);
    deleteTag();
    this.isLoading = false;
  }

  public onChangeFormTags(formTags: FormTag[]): void {
    this.formTags = formTags;
    this.tags = this.tagsService.toTags(this.formTags);
  }

  public async onClickSave(): Promise<void> {
    if (this.$validator.errors.count() > 1) {
      return;
    }

    if (this.isEditing) {
      this.updateFlag();
      return;
    }

    this.addFlag();
  }

  public async onClickDelete(): Promise<void> {
    if (!(await this.deleteFlag())) {
      return;
    }

    this.goBack();
  }

  private async getFlag(): Promise<void> {
    try {
      const { name, description, id, enabled, toggles } = await this.flagsService.detail(
        Number(this.id)
      );

      this.form.name = name;
      this.form.description = description;
      this.form.id = id;
      this.form.enabled = enabled;
      this.form.toggles = toggles;
    } catch (e) {
      this.$alert(this.$t('flags.errors.detail'), AlertType.Error);
    }
  }

  private async getTags(): Promise<void> {
    if (!this.form || !this.form.id) {
      return;
    }

    try {
      this.tags = await this.tagsService.get(this.form.id);
      this.formTags = await this.tagsService.toFormTags(this.tags);
    } catch (e) {
      this.$alert(this.$t('tags.errors.get'), AlertType.Error);
    }
  }

  private async addFlag(): Promise<void> {
    try {
      await this.flagsService.add({
        ...this.form,
        productId: Number(this.productId)
      });

      this.goBack();

      this.$alert(this.$t('flags.success.add'));
    } catch (e) {
      this.$alert(this.$t('flags.errors.add'), AlertType.Error);
    }
  }

  private async updateFlag(): Promise<void> {
    try {
      await this.flagsService.update(this.form);

      this.goBack();

      this.$alert(this.$t('flags.success.update'));
    } catch (e) {
      this.$alert(this.$t('flags.errors.update'), AlertType.Error);
    }
  }

  private async addFormTag(tag: FormTag): Promise<void> {
    const [newTag] = this.tagsService.toTags([tag]);
    await this.tagsService.add(Number(this.id), newTag);
  }

  private async removeFormTag(tag: FormTag): Promise<void> {
    const [removedTag] = this.tagsService.toTags([tag]);
    await this.tagsService.remove(Number(this.id), removedTag);
  }

  private isTagAllowed(tag: FormTag): boolean {
    if (this.formTags && this.formTags.find(x => x.text === tag.text)) {
      return false;
    }

    if (tag.tiClasses.includes('no-symbol')) {
      return false;
    }

    return true;
  }

  private async deleteFlag(): Promise<boolean> {
    if (
      !(await this.$confirm(
        this.$t('flags.confirm.title', [this.form.name])
      ))
    ) {
      return false;
    }

    try {
      const response = await this.flagsService.remove(this.form);
      this.$alert(this.$t('flags.success.delete'));

      return true;
    } catch (e) {
      this.$alert(this.$t('flags.errors.delete'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private goBack(): void {
    this.$router.push({
      name: 'products-edit',
      params: {
        id: this.productId
      }
    });
  }
}
</script>

<style lang="scss" scoped>
.flags_form {
  &-group {
    padding-left: 0;
  }

  &-switch {
    transform: translateY(.5rem);
  }
}
</style>
