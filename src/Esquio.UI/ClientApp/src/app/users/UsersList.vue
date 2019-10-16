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
        slot="managementPermission"
        slot-scope="data"
      >
        <div class="text-right">
          <router-link :to="{name: 'users-edit', params: {subjectId: data.item.subjectId}}">
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
      :to="{name: 'users-add'}"
    />
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import { AlertType } from '~/core';
import { FloatingTop, UserPermissions } from '~/shared';
import { IUsersPermissionsService } from './shared';

@Component({
  components: {
    FloatingTop
  }
})
export default class extends Vue {
  public name = 'UsersList';
  public usersPermissions: UserPermissions[] = null;
  public isLoading = true;
  public columns = [
    {
      key: 'subjectId',
      label: () => this.$t('users.fields.subject')
    },
    {
      key: 'managementPermission',
      label: ''
    }
  ];

  @Inject() usersPermissionsService: IUsersPermissionsService;

  public created(): void {
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
    if (!await this.$confirm(this.$t('users.confirm.title', [userPermissions.subjectId]))) {
      return;
    }

    try {
      const response = await this.usersPermissionsService.remove(userPermissions);
      this.usersPermissions = this.usersPermissions.filter(x => x.subjectId !== userPermissions.subjectId);
      this.$alert(this.$t('users.success.delete'));

    } catch (e) {
      this.$alert(this.$t('users.errors.delete'), AlertType.Error);

    } finally {
      this.isLoading = false;
    }
  }
}
</script>

