<template>
  <section class="products_list container u-container-medium">
    <h1>{{$t('products.title')}}</h1>
    <b-table
      striped
      hover
      :items="products"
      :fields="columns"
      :busy="isLoading"
      :empty-text="$t('common.empty')"
      :empty-filtered-text="$t('common.empty_filtered')"
      show-empty
    >
      <div
        slot="table-busy"
        class="text-center text-primary my-2"
      >
        <b-spinner class="align-middle"></b-spinner>
        <strong class="ml-2">{{$t('common.loading')}}</strong>
      </div>

      <template
        slot="empty"
        slot-scope="scope"
      >
        <div class="text-center">
          <h4 class="d-inline-block mr-3">{{ scope.emptyText }}</h4>
          <button
            class="btn btn-raised btn-primary d-inline-block"
            @click="onClickAddFirst"
          >
            {{$t('products.actions.add_first')}}
          </button>
        </div>
      </template>

      <template
        slot="id"
        slot-scope="data"
      >
        <div class="text-right">
          <router-link :to="{name: 'products-edit', params: {id: data.item.id}}">
            <button
              type="button"
              class="btn btn-sm btn-raised btn-primary"
            >
              {{$t('products.actions.see_detail')}}
            </button>
          </router-link>

          <button
            type="button"
            class="btn btn-sm btn-raised btn-danger ml-2"
            @click="onClickDelete(data.item)"
          >
            {{$t('products.actions.delete')}}
          </button>
        </div>
      </template>
    </b-table>

    <Floating
      :isTop="true"
      :text="$t('products.actions.add')"
      :to="{name: 'products-add'}"
    />
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { AlertType } from '~/core';
import { Floating } from '~/shared';
import { Product } from './product.model';
import { IProductsService } from './iproducts.service';

@Component({
  components: {
    Floating
  }
})
export default class extends Vue {
  public name = 'ProductsList';
  public products: Product[] = null;
  public isLoading = true;
  public columns = [
    {
      key: 'name',
      label: () => this.$t('products.fields.name')
    },
    {
      key: 'description',
      label: () => this.$t('products.fields.description')
    },
    {
      key: 'id',
      label: ''
    }
  ];

  @Inject() productsService: IProductsService;

  public created(): void {
    this.getProducts();
  }

  private async getProducts(): Promise<void> {
    try {
      const response = await this.productsService.get();
      this.products = response.result;
    } catch (e) {
      this.$alert(this.$t('products.errors.get'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  public async onClickDelete(product: Product): Promise<void> {
    await this.deleteProduct(product);
  }

  public async onClickAddFirst(): Promise<void> {
    await this.addDefaultProduct();
  }

  private async deleteProduct(product: Product): Promise<void> {
    if (!await this.$confirm(this.$t('products.confirm_delete.title', product.name))) {
      return;
    }

    try {
      const response = await this.productsService.remove(product);
      this.products = this.products.filter(x => x.id !== product.id);
      this.$alert(this.$t('products.success.delete'));

    } catch (e) {
      this.$alert(this.$t('products.errors.delete'), AlertType.Error);

    } finally {
      this.isLoading = false;
    }
  }

  private async addDefaultProduct(): Promise<void> {
    const defaultProduct: Product = {
      name: 'Default',
      description: 'Default Product'
    };

    try {
      await this.productsService.add(defaultProduct);

      this.$alert(this.$t('products.success.add'));
      await this.getProducts();
    } catch (e) {
      this.$alert(this.$t('products.errors.add'), AlertType.Error);
    }
  }
}
</script>

