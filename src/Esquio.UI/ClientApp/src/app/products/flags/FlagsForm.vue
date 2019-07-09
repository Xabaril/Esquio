<template>
  <section class="flags_form container u-container-medium">
    <div class="row">
      <h1 class="col col-auto pl-0">{{$t('flags.detail')}}</h1>
      <div class="flags_form-switch col col-auto pl-0">
        <custom-switch v-model="form.enabled" />
      </div>
    </div>
    <form class="row">
      <input-text
        class="flags_form-group form-group col-md-5"
        :class="{'is-disabled': isEditing}"
        v-model="form.name"
        id="flag_name"
        :label="$t('flags.fields.name')"
        validators="required|min:5"
        :help-label="$t('flags.placeholders.nameHelp')"
      />

      <input-text
        class="flags_form-group form-group col-md-5"
        :class="{'is-disabled': isEditing}"
        v-model="form.description"
        id="flag_description"
        :label="$t('flags.fields.description')"
        validators="required|min:5"
        :help-label="$t('flags.placeholders.descriptionHelp')"
      />
    </form>

    <div>
      <vue-tags-input
        v-model="formTag"
        :tags="formTags"
        @before-adding-tag="onAddFormTag"
        @before-deleting-tag="onRemoveFormTag"
        @tags-changed="onChangeFormTags"
      />
      {{this.tags}}
    </div>

    <Floating
      v-if="!this.isEditing"
      :text="$t('flags.actions.save')"
      :icon="floatingIcon"
      :disabled="isSaveActionDisabled"
      @click="onClickSave"
    />
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import VueTagsInput from '@johmun/vue-tags-input';
import { Inject } from 'inversify-props';
import { Floating, FloatingIcon, InputText, CustomSwitch } from '~/shared';
import { ITagsService, Tag, FormTag } from '~/products/shared/tags';
import { Flag } from './flag.model';
import { IFlagsService } from './iflags.service';

@Component({
  components: {
    Floating,
    InputText,
    CustomSwitch,
    VueTagsInput
  }
})
export default class extends Vue {
  public name = 'FlagsForm';
  public floatingIcon = FloatingIcon.Save;
  public isLoading = false;
  public tags: Tag[] = null;
  public formTags: FormTag[] = [];
  public formTag = '';
  public form: Flag = {
    productId: null,
    id: null,
    name: null,
    description: null,
    enabled: false
  };

  @Inject() flagsService: IFlagsService;
  @Inject() tagsService: ITagsService;

  @Prop() id: string;
  @Prop({ required: true }) productId: string;

  get isEditing(): boolean {
    return !!this.id;
  }

  get isSaveActionDisabled(): boolean {
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
  }

  public onAddFormTag({ tag, addTag }): void {
    this.addFormTag(tag);
    addTag();
  }

  public onRemoveFormTag({ tag, deleteTag }): void {
    this.removeFormTag(tag);
    deleteTag();
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

  private async getFlag(): Promise<void> {
    try {
      const { name, description, id, enabled } = await this.flagsService.detail(
        Number(this.id)
      );

      this.form.name = name;
      this.form.description = description;
      this.form.id = id;
      this.form.enabled = enabled;
    } catch (e) {
      this.$toasted.global.error({
        message: this.$t('flags.errors.detail')
      });
    } finally {
      this.isLoading = false;
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
      this.$toasted.global.error({
        message: this.$t('tags.errors.get')
      });
    } finally {
      this.isLoading = false;
    }
  }

  private async addFlag(): Promise<void> {
    try {
      await this.flagsService.add({
        ...this.form,
        productId: Number(this.productId)
      });

      this.$router.push({
        name: 'products-edit',
        params: {
          id: this.productId
        }
      });

      this.$toasted.global.success({
        message: this.$t('flags.success.add')
      });
    } catch (e) {
      this.$toasted.global.error({
        message: this.$t('flags.errors.add')
      });
    }
  }

  private async updateFlag(): Promise<void> {
    try {
      await this.flagsService.update(this.form);

      this.$router.push({
        name: 'products-edit',
        params: {
          id: this.productId
        }
      });

      this.$toasted.global.success({
        message: this.$t('flags.success.update')
      });
    } catch (e) {
      this.$toasted.global.error({
        message: this.$t('flags.errors.update')
      });
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
}
</script>

<style lang="scss" scoped>
.flags_form {
  &-group {
    padding-left: 0;

    &.is-disabled {
      pointer-events: none;
    }
  }

  &-switch {
    transform: translateY(.5rem);
  }
}
</style>
