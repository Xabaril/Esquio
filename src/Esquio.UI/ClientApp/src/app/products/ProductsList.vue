<template>
  <section class="products_list">
    <b-table
      striped
      hover
      :items="items"
      :fields="fields"
    ></b-table>

    <Floating :text="$t('products.actions.add')" :to="{name: 'products-add'}"/>
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Floating } from '~/shared';
import { Product } from './product.model';

@Component({
  components: {
    Floating
  }
})
export default class extends Vue {
  public name = 'ProductsList';
  public fields;
  public items: Product[] = [
    { id: '1xk23', name: 'My first product'},
    { id: '1xk24', name: 'My second product'},
    { id: '1xk25', name: 'My third product'},
    { id: '1xk26', name: 'My fourth product'},
    { id: '1xk27', name: 'My fifth product'}
  ];

  public created() {
    this.fields = [
    {
      key: 'id',
      label: this.$t('products.fields.id'),
      sortable: true
    },
    {
      key: 'name',
      label: this.$t('products.fields.name')
    }
  ];
  }
}
</script>

