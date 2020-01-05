<template>
  <section class="tokens_list container u-container-medium">
    <h1>{{$t('tokens.title')}}</h1>
    <b-table
      striped
      hover
      :items="tokens"
      :fields="columns"
      :busy="isLoading"
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
        slot="managementPermission"
        slot-scope="data"
      >
        <div class="text-right">
          <button
            type="button"
            class="btn btn-sm btn-raised btn-danger ml-2"
            @click="onClickDelete(data.item)"
          >
            {{$t('tokens.actions.delete')}}
          </button>
        </div>
      </template>
    </b-table>

    <b-pagination
      v-model="paginationInfo.pageIndex"
      :total-rows="paginationInfo.rows"
      :per-page="paginationInfo.pageCount"
      @change="onChangePage"
      align="right"
    ></b-pagination>

    <FloatingTop
      :text="$t('tokens.actions.add')"
      :to="{name: 'tokens-add'}"
    />
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { AlertType } from '~/core';
import { FloatingTop, UserPermissions, PaginationInfo } from '~/shared';
import { ITokensService } from './itokens.service';
import { Token } from './token.model';

@Component({
  components: {
    FloatingTop
  }
})
export default class extends Vue {
  public name = 'UsersList';
  public tokens: Token[] = null;
  public isLoading = true;
  public paginationInfo = new PaginationInfo();
  public columns = [
    {
      key: 'subjectId',
      label: () => this.$t('tokens.fields.subject')
    },
    {
      key: 'managementPermission',
      label: ''
    }
  ];

  @Inject() tokensService: ITokensService;

  public created(): void {
    this.getTokens();
  }

  public async onClickDelete(token: Token): Promise<void> {
    await this.deleteToken(token);
  }

  public onChangePage(page: number): void {
    this.tokens = null;
    this.isLoading = true;
    this.paginationInfo.pageIndex = page - 1;
    this.getTokens();
  }

  private async getTokens(): Promise<void> {
    try {
      const response = await this.tokensService.get(this.paginationInfo);
      this.tokens = response.result;
      this.paginationInfo.rows = response.total;
      this.paginationInfo.pageIndex = response.pageIndex + 1;
    } catch (e) {
      this.$alert(this.$t('tokens.errors.get'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async deleteToken(token: Token): Promise<void> {
    if (!await this.$confirm(this.$t('tokens.confirm.title', [token.id]))) {
      return;
    }

    try {
      // const response = await this.tokensService.remove(token);
      this.tokens = this.tokens.filter(x => x.id !== token.id);
      this.$alert(this.$t('tokens.success.delete'));
    } catch (e) {
      this.$alert(this.$t('tokens.errors.delete'), AlertType.Error);

    } finally {
      this.isLoading = false;
    }
  }
}
</script>

