<template>
  <section class="products_list container u-container-medium">
    <b-table
      striped
      hover
      :items="items"
      :fields="fields"
    >
      <div
        slot="table-busy"
        class="text-center text-danger my-2"
      >
        <b-spinner class="align-middle"></b-spinner>
        <strong>Loading...</strong>
      </div>

      <template
        slot="id"
        slot-scope="data"
      >
        <router-link :to="{name: 'products-edit', params: {id: data.item.id}}">{{$t('products.actions.see_detail')}}</router-link>
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
import { Floating } from '~/shared';
import { Product } from './product.model';
import { Inject } from 'inversify-props';
import { IProductsService } from './iproducts.service';

@Component({
  components: {
    Floating
  }
})
export default class extends Vue {
  public name = 'ProductsList';
  public fields = [
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
      label: () => this.$t('products.fields.id')
    }
  ];

  @Inject() productsService: IProductsService;

  public items: Product[] = [
    { id: '1xk23', name: 'My first product', description: 'Lorem Ipsum'},
    { id: '1xk24', name: 'My second product', description: 'Lorem Ipsum'},
    { id: '1xk25', name: 'My third product', description: 'Lorem Ipsum'},
    { id: '1xk26', name: 'My fourth product', description: 'Lorem Ipsum'},
    { id: '1xk27', name: 'My fifth product', description: 'Lorem Ipsum'}
  ];

  mounted() {
    this.productsService.get();
  }
}
</script>

