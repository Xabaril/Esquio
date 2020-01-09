<template>
  <section class="products_list container u-container-medium">
    <h1>{{$t('products.title')}}</h1>

    <PaginatedTable
      :fields="columns"
      :items="products"
      :busy="isLoading"
      :paginationInfo="paginationInfo"
      @change-page="onChangePage">
        <template slot="empty">
          <button
              v-if="$can($constants.AbilityAction.Create, $constants.AbilitySubject.Product)"
              class="btn btn-raised btn-primary d-inline-block"
              @click="onClickAddFirst"
            >
              {{$t('products.actions.add_first')}}
          </button>
        </template>

        <template
          slot="actions"
          slot-scope="data"
        >
          <div
            v-if="$can($constants.AbilityAction.Read, $constants.AbilitySubject.Product)"
            class="text-right">
            <router-link :to="{name: 'products-edit', params: {productName: data.item.name}}">
              <button type="button" class="btn btn-sm btn-raised btn-primary">
                {{$t('products.actions.see_detail')}}
              </button>
            </router-link>

            <button
              v-if="$can($constants.AbilityAction.Delete, $constants.AbilitySubject.Product)"
              type="button"
              class="btn btn-sm btn-raised btn-danger ml-2"
              @click="onClickDelete(data.item)"
            >
              {{$t('products.actions.delete')}}
            </button>
          </div>
        </template>
    </PaginatedTable>

    <FloatingTop
      v-if="$can($constants.AbilityAction.Create, $constants.AbilitySubject.Product)"
      :text="$t('products.actions.add')"
      :to="{name: 'products-add'}"
    />
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { AlertType } from '~/core';
import { FloatingTop, PaginationInfo, PaginatedTable } from '~/shared';
import { Product } from './product.model';
import { IProductsService } from './iproducts.service';

@Component({
  components: {
    FloatingTop,
    PaginatedTable
  }
})
export default class extends Vue {
  public name = 'ProductsList';
  public products: Product[] = null;
  public isLoading = true;
  public paginationInfo = new PaginationInfo();

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
      key: 'actions',
      label: ''
    }
  ];

  @Inject() productsService: IProductsService;

  public created(): void {
    this.getProducts();
  }

  public async onClickDelete(product: Product): Promise<void> {
    await this.deleteProduct(product);
  }

  public async onClickAddFirst(): Promise<void> {
    await this.addDefaultProduct();
  }

  public onChangePage(page: number): void {
    this.products = null;
    this.isLoading = true;
    this.paginationInfo.pageIndex = page;
    this.getProducts();
  }

  private async getProducts(): Promise<void> {
    try {
      const response = await this.productsService.get(this.paginationInfo);
      this.products = response.result;
      this.paginationInfo.rows = response.total;
      this.paginationInfo.pageIndex = response.pageIndex;
    } catch (e) {
      this.$alert(this.$t('products.errors.get'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async deleteProduct(product: Product): Promise<void> {
    if (!await this.$confirm(this.$t('products.confirm.title', [product.name]))) {
      return;
    }

    try {
      const response = await this.productsService.remove(product);
      this.products = this.products.filter(x => x.name !== product.name);
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

