<template>
  <section class="products_form container u-container-medium">
    <form>
      <div class="form-group">
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
    </form>

    <Floating :text="$t('products.actions.save')" :modifier="floatingModifier" :icon="floatingIcon"/>
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Floating, FloatingModifier, FloatingIcon } from '~/shared';
import { Product } from './product.model';

@Component({
  components: {
    Floating
  }
})
export default class extends Vue {
  public name = 'ProductsForm';
  public product: Product;
  public floatingModifier = FloatingModifier.Success;
  public floatingIcon = FloatingIcon.Save;

  @Prop() id: string;
}
</script>

<style lang="scss" scoped>
.products_form {
}
</style>
