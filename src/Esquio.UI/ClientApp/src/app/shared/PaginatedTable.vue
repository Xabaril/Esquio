<template>
  <div>
    <b-table
      striped
      hover
      :items="items"
      :fields="columns"
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
        slot="extra"
        slot-scope="data"
      >

        <slot name="extra" :item="data.item" />
      </template>

      <template
        slot="actions"
        slot-scope="data"
      >

        <slot name="actions" :item="data.item" />
      </template>

      <slot/>
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
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import { BvTableFieldArray } from 'bootstrap-vue';
import { PaginationInfo } from './pagination';

@Component
export default class extends Vue {
  public name = 'PaginatedTable';
  // TODO: Make extra and actions more generic, see adutilist
  public columns: BvTableFieldArray = [
    {
      key: 'extra',
      label: ''
    },
    {
      key: 'actions',
      label: ''
    }
  ];

  @Prop({ required: true, type: Array }) fields: BvTableFieldArray;
  @Prop({ type: Array, default: () => [] }) items: Array<any>;
  @Prop({ type: Boolean, default: false }) busy: boolean;
  @Prop({ type: PaginationInfo, default: () => new PaginationInfo() }) paginationInfo: PaginationInfo;

  public created(): void {
    this.updateColumns();
  }

  public onChangePage(page: number): void {
    this.$emit('change-page', page);
  }

  public get showPagination(): boolean {
    return this.paginationInfo.rows > this.paginationInfo.pageCount;
  }

  private updateColumns(): void {
    this.columns = [...this.fields, ...this.columns];
    this.columns = this.columns.map((column: any) => {
      column.label = column.label ? this.$t(column.label) : column.label;
      return column;
    });
  }

  @Watch('fields') onChangefields() {
    this.updateColumns();
  }
}
</script>

