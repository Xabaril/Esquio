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

    <div class="row" v-if="isEditing">
      <h2>{{$t('flags.title')}}</h2>
      <FlagsList :productId="id"/>
    </div>

    <Floating
      :text="$t('products.actions.save')"
      :icon="floatingIcon"
      :disabled="isSaveActionDisabled"
      @click="onClickSave"
    />

    <Floating
      v-if="isEditing"
      :isTop="true"
      :text="$t('products.actions.add_flag')"
      :to="{name: 'flags-add', params: { productId: form.id } }"
    />
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { Floating, FloatingIcon, InputText } from '~/shared';
import { Product } from './product.model';
import { FlagsList } from './flags';
import { IProductsService } from './iproducts.service';

@Component({
  components: {
    Floating,
    InputText,
    FlagsList
  }
})
export default class extends Vue {
  public name = 'ProductsForm';
  public floatingIcon = FloatingIcon.Save;
  public isLoading = false;
  public form: Product = { id: null, name: null, description: null };

  @Inject() productsService: IProductsService;

  @Prop() id: string;

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
    await this.getProduct();
  }

  public async getProduct(): Promise<void> {
    try {
      const { name, description, id } = await this.productsService.detail(
        Number(this.id)
      );

      this.form.name = name;
      this.form.description = description;
      this.form.id = id;
    } catch (e) {
      this.$toasted.global.error({
        message: this.$t('products.errors.detail')
      });
    } finally {
      this.isLoading = false;
    }
  }

  public async addProduct(): Promise<void> {
    try {
      await this.productsService.add(this.form);

      this.$router.push({
        name: 'products-list'
      });

      this.$toasted.global.success({
        message: this.$t('products.success.add')
      });
    } catch (e) {
      this.$toasted.global.error({
        message: this.$t('products.errors.add')
      });
    }
  }

  public async updateProduct(): Promise<void> {
    try {
      await this.productsService.update(this.form);

      this.$router.push({
        name: 'products-list'
      });

      this.$toasted.global.success({
        message: this.$t('products.success.update')
      });
    } catch (e) {
      this.$toasted.global.error({
        message: this.$t('products.errors.update')
      });
    }
  }

  public async onClickSave(): Promise<void> {
    if (this.$validator.errors.count() > 1) {
      return;
    }

    if (this.isEditing) {
      this.updateProduct();
      return;
    }

    this.addProduct();
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
