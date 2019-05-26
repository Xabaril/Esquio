<template>
  <section class="products_form container u-container-medium">
    <form class="row">
      <div class="products_form-group form-group col-md-5">
        <label for="name">{{$t('products.fields.name')}}</label>
        <input
          type="text"
          class="form-control"
          id="name"
          aria-describedby="nameHelp"
          :placeholder="$t('products.placeholders.name')"
        >
        <small
          id="nameHelp"
          class="form-text text-muted"
        >{{$t('products.placeholders.nameHelp')}}</small>
      </div>

      <div class="products_form-group form-group col-md-7">
        <label for="description">{{$t('products.fields.description')}}</label>
        <input
          type="text"
          class="form-control"
          id="description"
          aria-describedby="descriptionHelp"
          :placeholder="$t('products.placeholders.description')"
        >
        <small
          id="descriptionHelp"
          class="form-text text-muted"
        >{{$t('products.placeholders.descriptionHelp')}}</small>
      </div>
    </form>

    <div class="row" v-if="hasFeatures">
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
      </b-table>
    </div>

    <Floating
      :text="$t('products.actions.save')"
      :icon="floatingIcon"
    />

    <Floating
      v-if="isEditing"
      :isTop="true"
      :text="$t('products.actions.addFeature')"
      :to="{name: 'features-add'}"
    />
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Floating, FloatingIcon } from '~/shared';
import { Product } from './product.model';
import { Feature } from './features';

@Component({
  components: {
    Floating
  }
})
export default class extends Vue {
  public name = 'ProductsForm';
  public product: Product;
  public floatingIcon = FloatingIcon.Save;

  @Prop() id: string;

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

  // in progress
  public items: Feature[] = [
    { id: '1xj23', name: 'My first product', description: 'Lorem Ipsum', enabled: false, rollout: false},
    { id: '1xj24', name: 'My second product', description: 'Lorem Ipsum', enabled: false, rollout: false},
    { id: '1xj25', name: 'My third product', description: 'Lorem Ipsum', enabled: false, rollout: false},
    { id: '1xj26', name: 'My fourth product', description: 'Lorem Ipsum', enabled: false, rollout: false},
    { id: '1xj27', name: 'My fifth product', description: 'Lorem Ipsum', enabled: false, rollout: false}
  ];

  get isEditing(): boolean {
    return !!this.id;
  }

  get hasFeatures(): boolean {
    return this.isEditing && this.product && !!this.product.features;
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
