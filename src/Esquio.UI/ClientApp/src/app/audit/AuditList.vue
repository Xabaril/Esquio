<template>
  <section class="audit_list container u-container-medium">
    <h1>{{$t('audit.title')}}</h1>

    <PaginatedTable
      :fields="columns"
      :items="audit"
      :busy="isLoading"
      :paginationInfo="paginationInfo"
      @change-page="onChangePage"
    >
      <template
        slot="extra"
        slot-scope="data"
      >
        <div class="audit_list-code row">
          <vue-json-pretty class="col-6" :show-line="true" :highlight-mouseover-node="true"  :data="JSON.parse(data.item.oldValues)" />
          <vue-json-pretty class="col-6" :show-line="true" :highlight-mouseover-node="true" :data="JSON.parse(data.item.newValues)" />
        </div>
      </template>
    </PaginatedTable>
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { AlertType } from '~/core';
import {
  UserPermissions,
  PaginationInfo,
  IDateService,
  PaginatedTable
} from '~/shared';
import { IAuditService } from './iaudit.service';
import { AuditItem } from './audit-item.model';
import VueJsonPretty from 'vue-json-pretty';

@Component({
  components: {
    VueJsonPretty,
    PaginatedTable
  }
})
export default class extends Vue {
  public name = 'AuditList';
  public audit: AuditItem[] = null;
  public isLoading = true;
  public paginationInfo = new PaginationInfo();
  public columns = [
    {
      key: 'productName',
      label: 'audit.fields.productName'
    },
    {
      key: 'featureName',
      label: 'audit.fields.featureName'
    },
    {
      key: 'extra',
      label: 'audit.fields.values'
    }
  ];

  @Inject() auditService: IAuditService;
  @Inject() dateService: IDateService;

  public created(): void {
    this.getAudit();
  }

  public onChangePage(page: number): void {
    this.audit = null;
    this.isLoading = true;
    this.paginationInfo.pageIndex = page;
    this.getAudit();
  }

  private async getAudit(): Promise<void> {
    try {
      const response = await this.auditService.get(this.paginationInfo);
      this.audit = response.result;
      this.paginationInfo.rows = response.total;
      this.paginationInfo.pageIndex = response.pageIndex;
    } catch (e) {
      this.$alert(this.$t('audit.errors.get'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }
}
</script>

<style lang="scss" scoped>
.audit_list {
  &-code {
    /deep/ .vjs-tree {
      font-size: get-font-size(s);
    }
  }
}
</style>
