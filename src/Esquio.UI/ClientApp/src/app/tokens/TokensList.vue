<template>
  <section class="tokens_list container u-container-medium">
    <div class="col">
      <TokensForm @add="onAddToken" />
    </div>
    <h1>{{$t('tokens.title')}}</h1>

    <PaginatedTable
      :fields="columns"
      :items="tokens"
      :busy="isLoading"
      :paginationInfo="paginationInfo"
      @change-page="onChangePage"
    >
      <template
        slot="actions"
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

      <template
        slot="extra"
        slot-scope="data"
      >
        <div>
          {{formatDate(data.item.validTo)}}
        </div>
      </template>
    </PaginatedTable>
  </section>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { Inject } from "inversify-props";
import { AlertType } from "~/core";
import {
  UserPermissions,
  PaginationInfo,
  IDateService,
  PaginatedTable
} from "~/shared";
import { ITokensService } from "./itokens.service";
import { default as TokensForm } from "./TokensForm.vue";
import { Token } from "./token.model";

@Component({
  components: {
    TokensForm,
    PaginatedTable
  }
})
export default class extends Vue {
  public name = "TokensList";
  public tokens: Token[] = null;
  public isLoading = true;
  public paginationInfo = new PaginationInfo();
  public columns = [
    {
      key: "name",
      label: "tokens.fields.name"
    },
    {
      key: "extra",
      label: "tokens.fields.validTo"
    }
  ];

  @Inject() tokensService: ITokensService;
  @Inject() dateService: IDateService;

  public created(): void {
    this.getTokens();
  }

  public async onClickDelete(token: Token): Promise<void> {
    await this.deleteToken(token);
  }

  public onChangePage(page: number): void {
    this.tokens = null;
    this.isLoading = true;
    this.paginationInfo.pageIndex = page;
    this.getTokens();
  }

  public onAddToken(): void {
    this.getTokens();
  }

  private async getTokens(): Promise<void> {
    try {
      const response = await this.tokensService.get(this.paginationInfo);
      this.tokens = response.result;
      this.paginationInfo.rows = response.total;
      this.paginationInfo.pageIndex = response.pageIndex;
    } catch (e) {
      this.$alert(this.$t("tokens.errors.get"), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async deleteToken(token: Token): Promise<void> {
    if (!(await this.$confirm(this.$t("tokens.confirm.title", [token.name])))) {
      return;
    }

    try {
      const response = await this.tokensService.remove(token);
      this.tokens = this.tokens.filter(x => x.name !== token.name);
      this.$alert(this.$t("tokens.success.delete"));
    } catch (e) {
      this.$alert(this.$t("tokens.errors.delete"), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private formatDate(validTo: Date): string {
    return this.dateService.fancyFormatDateTime(validTo);
  }
}
</script>

