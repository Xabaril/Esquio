<template>
  <div>
    <b-table
      striped
      hover
      :items="items"
      :fields="fields"
      :busy="busy"
      :empty-text="$t('common.empty')"
      :empty-filtered-text="$t('common.empty_filtered')"
      show-empty
      :per-page="0"
      :current-page="paginationInfo.pageIndex"
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
          <slot name="empty" />
        </div>
      </template>

      <template
        slot="actions"
        slot-scope="data"
      >

        <slot name="actions" :item="data.item" />
      </template>
    </b-table>

    <b-pagination
      v-if="showPagination"
      v-model="paginationInfo.pageIndex"
      :total-rows="paginationInfo.rows"
      :per-page="paginationInfo.pageCount"
      @change="onChangePage"
      align="right"
    ></b-pagination>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { BvTableFieldArray } from 'bootstrap-vue';
import { PaginationInfo } from './pagination';

@Component
export default class extends Vue {
  public name = 'PaginatedTable';

  @Prop({ required: true, type: Array }) fields: BvTableFieldArray;
  @Prop({ type: Array, default: () => [] }) items: Array<any>;
  @Prop({ type: Boolean, default: false }) busy: boolean;
  @Prop({ required: true, type: Object}) paginationInfo: PaginationInfo;

  public onChangePage(page: number): void {
    this.$emit('change-page', page);
  }

  public get showPagination(): boolean {
    return this.paginationInfo.rows > this.paginationInfo.pageCount;
  }
}
</script>

