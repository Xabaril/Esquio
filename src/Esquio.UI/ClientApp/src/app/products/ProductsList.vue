<template>
  <section class="products_list">
    <b-table
      striped
      hover
      :items="items"
      :fields="fields"
    ></b-table>

    <Floating />
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Floating } from '~/shared';

@Component({
  components: {
    Floating
  }
})
export default class extends Vue {
  public name = 'ProductsList';

  fields = [
    {
      key: 'first_name',
      label: 'col 1',
      sortable: true
    },
    {
      key: 'last_name',
    },
    {
      key: 'age',
    }
  ];

  items = [
    { age: 40, first_name: 'Dickerson', last_name: 'Macdonald' },
    { age: 21, first_name: 'Larsen', last_name: 'Shaw' },
    { age: 89, first_name: 'Geneva', last_name: 'Wilson' },
    { age: 38, first_name: 'Jami', last_name: 'Carney' }
  ];
}
</script>

<style lang="scss" scoped>
.products {
  &-header {
    color: get-color(primary);
  }
}
</style>
