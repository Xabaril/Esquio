<template>
  <section class="users_list container u-container-medium">
    <h1>{{$t('users.title')}}</h1>

    <PaginatedTable
      :fields="columns"
      :items="usersPermissions"
      :busy="isLoading"
      :paginationInfo="paginationInfo"
      @change-page="onChangePage"
    >
      <template
        slot="actions"
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
    </PaginatedTable>

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
import { FloatingTop, UserPermissions, PaginationInfo, PaginatedTable } from '~/shared';
import { IUsersPermissionsService } from './shared';

@Component({
  components: {
    FloatingTop,
    PaginatedTable
  }
})
export default class extends Vue {
  public name = 'UsersList';
  public usersPermissions: UserPermissions[] = null;
  public isLoading = true;
  public paginationInfo = new PaginationInfo();
  public columns = [
    {
      key: 'subjectId',
      label: 'users.fields.subject'
    }
  ];

  @Inject() usersPermissionsService: IUsersPermissionsService;

  public created(): void {
    this.getUsersPermissions();
  }

  public async onClickDelete(userPermissions: UserPermissions): Promise<void> {
    await this.deleteUserPermissions(userPermissions);
  }

  public onChangePage(page: number): void {
    this.usersPermissions = null;
    this.isLoading = true;
    this.paginationInfo.pageIndex = page;
    this.getUsersPermissions();
  }

  private async getUsersPermissions(): Promise<void> {
    try {
      const response = await this.usersPermissionsService.get(this.paginationInfo);
      this.usersPermissions = response.result;
      this.paginationInfo.rows = response.total;
      this.paginationInfo.pageIndex = response.pageIndex;
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

