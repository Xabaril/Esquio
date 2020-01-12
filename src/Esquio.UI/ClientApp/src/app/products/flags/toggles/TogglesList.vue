<template>
  <section class="toggles_list container">
    <PaginatedTable
      :fields="columns"
      :items="toggles"
      :busy="isLoading"
    >
      <template slot="empty">
        <router-link
          v-if="$can($constants.AbilityAction.Create, $constants.AbilitySubject.Toggle)"
          class="btn btn-raised btn-primary d-inline-block"
          tag="button"
          :to="{name: 'toggles-add', params: { flagName: flagName }}"
        >
          {{$t('toggles.actions.add_first')}}
        </router-link>
      </template>

      <template
        slot="actions"
        slot-scope="data"
      >
       <div
          v-if="$can($constants.AbilityAction.Read, $constants.AbilitySubject.Toggle)"
          class="text-right">
          <router-link :to="{ name: 'toggles-edit', params: { type: data.item.type, productName, flagName }}">
            <button
              type="button"
              class="btn btn-sm btn-raised btn-primary"
            >
              {{$t('toggles.actions.see_detail')}}
            </button>
          </router-link>

          <button
            v-if="$can($constants.AbilityAction.Delete, $constants.AbilitySubject.Toggle)"
            type="button"
            class="btn btn-sm btn-raised btn-danger ml-2"
            @click="onClickDelete(data.item)"
          >
            {{$t('toggles.actions.delete')}}
          </button>
        </div>
      </template>
    </PaginatedTable>
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { CustomSwitch, PaginatedTable } from '~/shared';
import { AlertType } from '~/core';
import { Toggle } from './toggle.model';
import { ITogglesService } from './itoggles.service';

@Component({
  components: {
    CustomSwitch,
    PaginatedTable
  }
})
export default class extends Vue {
  public name = 'TogglesList';
  public isLoading = false;
  public columns = [
    {
      key: 'friendlyName',
      label: 'toggles.fields.type'
    }
  ];

  @Inject() togglesService: ITogglesService;

  @Prop({ required: true, type: String }) flagName: string;
  @Prop({ required: true, type: String }) productName: string;
  @Prop({ type: Array }) toggles: Toggle[];

  public async onClickDelete(toggle: Toggle): Promise<void> {
    await this.deleteToggle(toggle);
  }

  private async deleteToggle(toggle: Toggle): Promise<void> {
    if (!await this.$confirm(this.$t('toggles.confirm.title', [toggle.friendlyName]))) {
      return;
    }

    try {
      await this.togglesService.remove(this.productName, this.flagName, toggle);
      this.toggles = this.toggles.filter(x => x.type !== toggle.type);
      this.$alert(this.$t('toggles.success.delete'));
    } catch (e) {
      this.$alert(this.$t('toggles.errors.delete'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }
}
</script>

