<template>
  <section class="products_form container u-container-medium pl-0 pr-0">
    <div class="row">
      <h1>{{$t('products.detail')}}</h1>
    </div>
    <form class="row">
      <input-text
        class="products_form-group form-group col-md-6"
        v-model="form.name"
        id="product_name"
        :label="$t('products.fields.name')"
        validators="required|min:5"
        :help-label="$t('products.placeholders.nameHelp')"
      />

      <input-text
        class="products_form-group form-group col-md-6"
        v-model="form.description"
        id="product_description"
        :label="$t('products.fields.description')"
        validators="required|min:5"
        :help-label="$t('products.placeholders.descriptionHelp')"
      />
    </form>

    <div
      class="row"
      v-if="isEditing"
    >
      <h2>{{$t('flags.title')}}</h2>
      <FlagsList :productId="id" />
    </div>

    <FloatingContainer>
      <FloatingDelete
        v-if="isEditing && $can($constants.AbilityAction.Delete, $constants.AbilitySubject.Product)"
        :text="$t('products.actions.delete')"
        :disabled="areActionsDisabled"
        @click="onClickDelete"
      />

      <FloatingSave
        v-if="$can($constants.AbilityAction.Update, $constants.AbilitySubject.Product)"
        :text="$t('products.actions.save')"
        :disabled="areActionsDisabled"
        @click="onClickSave"
      />
    </FloatingContainer>

    <FloatingTop
      v-if="isEditing && $can($constants.AbilityAction.Create, $constants.AbilitySubject.Flag)"
      :text="$t('products.actions.add_flag')"
      :to="{name: 'flags-add', params: { productId: form.id }}"
    />
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import {
  FloatingSave,
  FloatingTop,
  FloatingDelete,
  InputText,
  FloatingModifier,
  FloatingContainer
} from '~/shared';
import { AlertType } from '~/core';
import { Product } from './product.model';
import { FlagsList } from './flags';
import { IProductsService } from './iproducts.service';

@Component({
  components: {
    FloatingSave,
    FloatingTop,
    FloatingDelete,
    FloatingContainer,
    InputText,
    FlagsList
  }
})
export default class extends Vue {
  public name = 'ProductsForm';
  public isLoading = false;
  public form: Product = { id: null, name: null, description: null };

  @Inject() productsService: IProductsService;

  @Prop({ type: [String, Number]}) id: string;

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

    await this.getProduct();
  }

  public async onClickSave(): Promise<void> {
    if (this.$validator.errors.count() > 1) {
      return;
    }

    if (!this.isEditing) {
      await this.addProduct();
    } else {
      await this.updateProduct();
    }

    this.goBack();
  }

  public async onClickDelete(): Promise<void> {
    if (!(await this.deleteProduct())) {
      return;
    }

    this.goBack();
  }

  private async getProduct(): Promise<void> {
    this.isLoading = true;
    try {
      const { name, description, id } = await this.productsService.detail(
        Number(this.id)
      );

      this.form.name = name;
      this.form.description = description;
      this.form.id = id;
    } catch (e) {
      this.$alert(this.$t('products.errors.detail'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async addProduct(): Promise<void> {
    try {
      await this.productsService.add(this.form);

      this.$alert(this.$t('products.success.add'));
    } catch (e) {
      this.$alert(this.$t('products.errors.add'), AlertType.Error);
    }
  }

  private async updateProduct(): Promise<void> {
    try {
      await this.productsService.update(this.form);

      this.$alert(this.$t('products.success.update'));
    } catch (e) {
      this.$alert(this.$t('products.errors.update'), AlertType.Error);
    }
  }

  private async deleteProduct(): Promise<boolean> {
    if (
      !(await this.$confirm(
        this.$t('products.confirm.title', [this.form.name])
      ))
    ) {
      return false;
    }

    try {
      await this.productsService.remove(this.form);
      this.$alert(this.$t('products.success.delete'));
      return true;
    } catch (e) {
      this.$alert(this.$t('products.errors.delete'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private goBack(): void {
    this.$router.push({
      name: 'products-list'
    });
  }
}
</script>

<style lang="scss" scoped>
.products_form {
  &-group {
    padding-left: 0;
  }
}
</style>
