<template>
  <section class="users_list container u-container-medium">
    <h1>{{$t('users.title')}}</h1>
    <b-table
      striped
      hover
      :items="usersPermissions"
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
        slot="id"
        slot-scope="data"
      >
        <div class="text-right">
          <router-link :to="{name: 'users-edit', params: {id: data.item.id}}">
            <button
              type="button"
              class="btn btn-sm btn-raised btn-primary"
            >
              {{$t('users.actions.see_detail')}}
            </button>
          </router-link>

          <button
            type="button"
            class="btn btn-sm btn-raised btn-danger ml-2"
            @click="onClickDelete(data.item)"
          >
            {{$t('users.actions.delete')}}
          </button>
        </div>
      </template>
    </b-table>

    <FloatingTop
      :text="$t('users.actions.add')"
      :to="{name: 'products-add'}"
    />
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { AlertType } from '~/core';
import { FloatingTop } from '~/shared';
import { UserPermissions, IUsersPermissionsService } from './shared';

@Component({
  components: {
    FloatingTop
  }
})
export default class extends Vue {
  public name = 'ProductsList';
  public usersPermissions: UserPermissions[] = null;
  public isLoading = true;
  public columns = [
    {
      key: 'name',
      label: () => this.$t('products.fields.name')
    },
    {
      key: 'description',
      label: () => this.$t('products.fields.description')
    },
    {
      key: 'id',
      label: ''
    }
  ];

  @Inject() usersPermissionsService: IUsersPermissionsService;

  public created(): void {
    console.log(this.usersPermissionsService);
    this.getUsersPermissions();
  }

  public async onClickDelete(userPermissions: UserPermissions): Promise<void> {
    await this.deleteUserPermissions(userPermissions);
  }

  private async getUsersPermissions(): Promise<void> {
    try {
      const response = await this.usersPermissionsService.get();
      this.usersPermissions = response.result;
    } catch (e) {
      this.$alert(this.$t('users.errors.get'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async deleteUserPermissions(userPermissions: UserPermissions): Promise<void> {
    if (!await this.$confirm(this.$t('users.confirm.title', [userPermissions.id]))) {
      return;
    }

    try {
      const response = await this.usersPermissionsService.remove(userPermissions);
      this.usersPermissions = this.usersPermissions.filter(x => x.id !== userPermissions.id);
      this.$alert(this.$t('users.success.delete'));

    } catch (e) {
      this.$alert(this.$t('users.errors.delete'), AlertType.Error);

    } finally {
      this.isLoading = false;
    }
  }
}
</script>

