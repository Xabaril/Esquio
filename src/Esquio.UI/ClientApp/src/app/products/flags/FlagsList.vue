<template>
  <section class="flags_list container">
    <b-table
      striped
      hover
      :items="flags"
      :fields="columns"
      :busy="isLoading"
      :empty-text="$t('common.empty')"
      :empty-filtered-text="$t('common.empty_filtered')"
      show-empty
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
          <router-link
            class="btn btn-raised btn-primary d-inline-block"
            tag="button"
            :to="{name: 'flags-add', params: { productId }}"
          >
            {{$t('flags.actions.add_first')}}
          </router-link>
        </div>
      </template>

      <template
        slot="enabled"
        slot-scope="data"
      >
        <div class="text-center">
          <custom-switch v-model="data.item.enabled" @change="onClickFlagSwitch(data.item)"/>
        </div>
      </template>

      <template
        slot="id"
        slot-scope="data"
      >
        <div class="text-right">
          <router-link :to="{ name: 'flags-edit', params: { id: data.item.id, productId }}">
            <button
              type="button"
              class="btn btn-sm btn-raised btn-primary"
            >
              {{$t('flags.actions.see_detail')}}
            </button>
          </router-link>

          <button
            type="button"
            class="btn btn-sm btn-raised btn-danger ml-2"
            @click="onClickDelete(data.item)"
          >
            {{$t('flags.actions.delete')}}
          </button>
        </div>
      </template>
    </b-table>
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { CustomSwitch } from '~/shared';
import { Flag } from './flag.model';
import { IFlagsService } from './iflags.service';

@Component({
  components: {
    CustomSwitch
  }
})
export default class extends Vue {
  public name = 'FlagsList';
  public flags: Flag[] = null;
  public isLoading = true;
  public columns = [
    {
      key: 'name',
      label: () => this.$t('flags.fields.name')
    },
    {
      key: 'description',
      label: () => this.$t('flags.fields.description')
    },
    {
      key: 'enabled',
      label: ''
    },
    {
      key: 'id',
      label: ''
    }
  ];

  @Inject() flagsService: IFlagsService;

  @Prop({ required: true, type: [String, Number] }) productId: string;

  public created(): void {
    this.getFlags();
  }

  private async getFlags(): Promise<void> {
    try {
      const response = await this.flagsService.get(Number(this.productId));
      this.flags = response.result;
    } catch (e) {
      this.$toasted.global.error({
        message: this.$t('flags.errors.get')
      });
    } finally {
      this.isLoading = false;
    }
  }

  public async onClickDelete(flag: Flag): Promise<void> {
    await this.deleteFlag(flag);
  }

  public async onClickFlagSwitch(flag: Flag): Promise<void> {
    await this.updateFlagSwitch(flag);
  }

  private async deleteFlag(flag: Flag): Promise<void> {
    if (!(await this.$bvModal.msgBoxConfirm(this.$t('flags.confirm_delete.title', flag.name) as string))) {
      return;
    }

    try {
      const response = await this.flagsService.remove(flag);
      this.flags = this.flags.filter(x => x.id !== flag.id);
      this.$toasted.global.success({
        message: this.$t('flags.success.delete')
      });
    } catch (e) {
      this.$toasted.global.error({
        message: this.$t('flags.errors.delete')
      });
    } finally {
      this.isLoading = false;
    }
  }

  private async updateFlagSwitch(flag: Flag): Promise<void> {
    try {
      await this.flagsService.update(flag);

      this.$toasted.global.success({
        message: !flag.enabled ? this.$t('flags.success.off') : this.$t('flags.success.on')
      });
    } catch (e) {
      this.$toasted.global.error({
        message: this.$t('flags.errors.change')
      });
    } finally {
      this.isLoading = false;
    }
  }
}
</script>

