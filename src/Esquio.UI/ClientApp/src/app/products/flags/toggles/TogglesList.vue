<template>
  <section class="toggles_list container">
    <b-table
      striped
      hover
      :items="toggles"
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
            v-if="$can($constants.AbilityAction.Create, $constants.AbilitySubject.Toggle)"
            class="btn btn-raised btn-primary d-inline-block"
            tag="button"
            :to="{name: 'toggles-add', params: { id: flagId }}"
          >
            {{$t('toggles.actions.add_first')}}
          </router-link>
        </div>
      </template>

      <template
        slot="id"
        slot-scope="data"
      >
        <div
          v-if="$can($constants.AbilityAction.Read, $constants.AbilitySubject.Toggle)"
          class="text-right">
          <router-link :to="{ name: 'toggles-edit', params: { toggleId: data.item.id, id: flagId }}">
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
    </b-table>
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { CustomSwitch } from '~/shared';
import { AlertType } from '~/core';
import { Toggle } from './toggle.model';
import { ITogglesService } from './itoggles.service';

@Component({
  components: {
    CustomSwitch
  }
})
export default class extends Vue {
  public name = 'TogglesList';
  public isLoading = false;
  public columns = [
    {
      key: 'type',
      label: () => this.$t('toggles.fields.type')
    },
    {
      key: 'id',
      label: ''
    }
  ];

  @Inject() togglesService: ITogglesService;

  @Prop({ required: true, type: [Number, String] }) flagId: number;
  @Prop({ type: Array }) toggles: Toggle[];

  public async onClickDelete(toggle: Toggle): Promise<void> {
    await this.deleteToggle(toggle);
  }

  private async deleteToggle(toggle: Toggle): Promise<void> {
    if (!await this.$confirm(this.$t('toggles.confirm.title', [toggle.id]))) {
      return;
    }

    try {
      await this.togglesService.remove(toggle);
      this.toggles = this.toggles.filter(x => x.id !== toggle.id);
      this.$alert(this.$t('toggles.success.delete'));
    } catch (e) {
      this.$alert(this.$t('toggles.errors.delete'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }
}
</script>

