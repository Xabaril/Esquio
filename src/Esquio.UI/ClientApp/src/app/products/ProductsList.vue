<template>
  <section class="products_list container u-container-medium">
    <b-table
      striped
      hover
      :items="products"
      :fields="fields"
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
          <router-link
            class="btn btn-raised btn-primary d-inline-block"
            tag="button"
            :to="{name: 'products-add'}"
          >
            {{$t('products.actions.add_first')}}
          </router-link>
        </div>
      </template>

      <template
        slot="id"
        slot-scope="data"
      >
        <div class="text-center">
          <router-link :to="{name: 'products-edit', params: {id: data.item.id}}">{{$t('products.actions.see_detail')}}</router-link>
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
import { Component, Vue } from "vue-property-decorator";
import { Inject } from "inversify-props";
import { Floating } from "~/shared";
import { Product } from "./product.model";
import { IProductsService } from "./iproducts.service";

@Component({
  components: {
    Floating
  }
})
export default class extends Vue {
  public name = "ProductsList";
  public products: Product[] = null;
  public isLoading = true;
  public fields = [
    {
      key: "name",
      label: () => this.$t("products.fields.name")
    },
    {
      key: "description",
      label: () => this.$t("products.fields.description")
    },
    {
      key: "id",
      label: () => this.$t("products.fields.id")
    }
  ];

  @Inject() productsService: IProductsService;

  public created(): void {
    this.getProducts();
  }

  private async getProducts(): Promise<void> {
    const response = await this.productsService.get();
    // if error toaster...
    this.products = response.result;
    this.isLoading = false;
  }
}
</script>

