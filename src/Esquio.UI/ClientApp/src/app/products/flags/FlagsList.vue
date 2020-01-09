<template>
  <section class="flags_list container">

    <PaginatedTable
      :fields="columns"
      :items="flags"
      :busy="isLoading"
      :paginationInfo="paginationInfo"
      @change-page="onChangePage"
    >
      <template slot="empty">
        <router-link
          v-if="$can($constants.AbilityAction.Create, $constants.AbilitySubject.Flag)"
          class="btn btn-raised btn-primary d-inline-block"
          tag="button"
          :to="{name: 'flags-add', params: { productName }}"
        >
          {{$t('flags.actions.add_first')}}
        </router-link>
      </template>

      <template
        slot="actions"
        slot-scope="data"
      >
        <div
          v-if="$can($constants.AbilityAction.Read, $constants.AbilitySubject.Flag)"
          class="text-right"
        >
          <router-link :to="{ name: 'flags-edit', params: { flagName: data.item.name, productName }}">
            <button
              type="button"
              class="btn btn-sm btn-raised btn-primary"
            >
              {{$t('flags.actions.see_detail')}}
            </button>
          </router-link>

          <button
            type="button"
            class="btn btn-sm btn-raised btn-secondary ml-2"
            @click="onClickRollout(data.item)"
          >
            {{$t('flags.actions.rollout')}}
          </button>

          <button
            type="button"
            class="btn btn-sm btn-raised btn-secondary ml-2"
            @click="onClickRolloff(data.item)"
          >
            {{$t('flags.actions.rolloff')}}
          </button>

          <button
            v-if="$can($constants.AbilityAction.Delete, $constants.AbilitySubject.Flag)"
            type="button"
            class="btn btn-sm btn-raised btn-danger ml-2"
            @click="onClickDelete(data.item)"
          >
            {{$t('flags.actions.delete')}}
          </button>
        </div>
      </template>

      <template
        slot="extra"
        slot-scope="data"
      >
        <div class="text-center">
          <custom-switch
            v-model="data.item.enabled"
            @change="onClickFlagSwitch(data.item)"
          />
        </div>
      </template>
    </PaginatedTable>
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { CustomSwitch, PaginationInfo, PaginatedTable } from '~/shared';
import { AlertType } from '~/core';
import { Flag } from './flag.model';
import { IFlagsService } from './iflags.service';

@Component({
  components: {
    CustomSwitch,
    PaginatedTable
  }
})
export default class extends Vue {
  public name = 'FlagsList';
  public flags: Flag[] = null;
  public isLoading = true;
  public paginationInfo = new PaginationInfo();
  public columns = [
    {
      key: 'name',
      label: 'flags.fields.name'
    },
    {
      key: 'description',
      label: 'flags.fields.description'
    }
  ];

  @Inject() flagsService: IFlagsService;

  @Prop({ required: true, type: String }) productName: string;

  public created(): void {
    this.getFlags();
  }

  public async onClickDelete(flag: Flag): Promise<void> {
    await this.deleteFlag(flag);
  }

  public async onClickFlagSwitch(flag: Flag): Promise<void> {
    await this.updateFlagSwitch(flag);
  }

  public async onClickRollout(flag: Flag): Promise<void> {
    await this.rolloutFlag(flag);
  }

  public async onClickRolloff(flag: Flag): Promise<void> {
    await this.rolloffFlag(flag);
  }

  public onChangePage(page: number): void {
    this.flags = null;
    this.isLoading = true;
    this.paginationInfo.pageIndex = page;
    this.getFlags();
  }

  private async getFlags(): Promise<void> {
    try {
      const response = await this.flagsService.get(
        this.productName,
        this.paginationInfo
      );
      this.flags = response.result;
      this.paginationInfo.rows = response.total;
      this.paginationInfo.pageIndex = response.pageIndex;
    } catch (e) {
      this.$alert(this.$t('flags.errors.get'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async deleteFlag(flag: Flag): Promise<void> {
    if (!(await this.$confirm(this.$t('flags.confirm.title', [flag.name])))) {
      return;
    }

    try {
      await this.flagsService.remove(this.productName, flag);
      this.flags = this.flags.filter(x => x.name !== flag.name);
      this.$alert(this.$t('flags.success.delete'));
    } catch (e) {
      this.$alert(this.$t('flags.errors.delete'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async updateFlagSwitch(flag: Flag): Promise<void> {
    try {
      await this.flagsService.update(this.productName, flag, flag);

      this.$alert(
        !flag.enabled
          ? this.$t('flags.success.off')
          : this.$t('flags.success.on')
      );
    } catch (e) {
      this.$alert(this.$t('flags.errors.change'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async rolloutFlag(flag: Flag): Promise<void> {
    if (!(await this.$confirm(this.$t('flags.confirm.rollout', [flag.name])))) {
      return;
    }

    try {
      await this.flagsService.rollout(this.productName, flag);
      this.$alert(this.$t('flags.success.rollout'));
      this.getFlags();
    } catch (e) {
      this.$alert(this.$t('flags.errors.rollout'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async rolloffFlag(flag: Flag): Promise<void> {
    if (!(await this.$confirm(this.$t('flags.confirm.rolloff', [flag.name])))) {
      return;
    }

    try {
      await this.flagsService.rollback(this.productName, flag);
      this.$alert(this.$t('flags.success.rolloff'));
      this.getFlags();
    } catch (e) {
      this.$alert(this.$t('flags.errors.rolloff'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }
}
</script>

