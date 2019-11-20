<template>
  <section class="users_form container u-container-medium pl-0 pr-0">
    <div class="row">
      <h1>{{$t('users.detail')}}</h1>
    </div>
    <form class="row">
      <input-text
        class="users_form-group form-group col-md-6"
        :class="{'is-disabled': isEditing}"
        v-model="form.subjectId"
        id="user_name"
        :label="$t('users.fields.subject')"
        validators="required|min:1"
        :help-label="$t('users.placeholders.subjectHelp')"
      />

      <div class="users_form-group form-group col-md-6">
        <label for="role" class="bmd-label-floating">
          {{$t('users.placeholders.role')}}
        </label>
        <select
          class="users_form-select custom-select form-control"
          id="role"
          v-model="role"
        >
          <option :value="roles.Reader">{{$t(`users.roles.${roles.Reader}`)}}</option>
          <option :value="roles.Contributor">{{$t(`users.roles.${roles.Contributor}`)}}</option>
          <option :value="roles.Manager">{{$t(`users.roles.${roles.Manager}`)}}</option>
        </select>
      </div>
    </form>

    <FloatingContainer>
      <FloatingDelete
        v-if="isEditing"
        :text="$t('users.actions.delete')"
        :disabled="areActionsDisabled"
        @click="onClickDelete"
      />

      <FloatingSave
        :text="$t('users.actions.save')"
        :disabled="areActionsDisabled"
        @click="onClickSave"
      />
    </FloatingContainer>
  </section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Inject } from 'inversify-props';
import {
  FloatingSave,
  FloatingTop,
  FloatingDelete,
  InputText,
  FloatingModifier,
  FloatingContainer,
  UserPermissions
} from '~/shared';
import { AlertType } from '~/core';
import { IUsersPermissionsService } from './shared';

enum Role {
  Manager,
  Contributor,
  Reader
}

@Component({
  components: {
    FloatingSave,
    FloatingTop,
    FloatingDelete,
    FloatingContainer,
    InputText
  }
})
export default class extends Vue {
  public name = 'UsersForm';
  public isLoading = false;
  public form: UserPermissions = {
    subjectId: null,
    managementPermission: false,
    readPermission: false,
    writePermission: false
  };

  public roles = Role;
  public role: Role = null;

  @Inject() usersPermissionsService: IUsersPermissionsService;

  @Prop({ type: String }) subjectId: string;

  get isEditing(): boolean {
    return !!this.subjectId;
  }

  get areActionsDisabled(): boolean {
    return !this.form.subjectId || this.$validator.errors.count() > 0;
  }

  public async created(): Promise<void> {
    if (!this.isEditing) {
      return;
    }

    await this.getUserPermissions();
  }

  public async onClickSave(): Promise<void> {
    if (this.$validator.errors.count() > 1) {
      return;
    }

    if (!this.isEditing) {
      await this.addUserPermissions();
    } else {
      await this.updateUserPermissions();
    }

    this.goBack();
  }

  public async onClickDelete(): Promise<void> {
    if (!(await this.deleteUserPermissions())) {
      return;
    }

    this.goBack();
  }

  private async getUserPermissions(): Promise<void> {
    this.isLoading = true;
    try {
      const {
        subjectId,
        managementPermission,
        writePermission,
        readPermission
      } = await this.usersPermissionsService.detail(this.subjectId);

      this.form.subjectId = subjectId;
      this.form.managementPermission = managementPermission;
      this.form.writePermission = writePermission;
      this.form.readPermission = readPermission;
      this.processRolesAfterGet();
    } catch (e) {
      this.$alert(this.$t('products.errors.detail'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private async addUserPermissions(): Promise<void> {
    try {
      this.processRolesBeforePush();
      await this.usersPermissionsService.add(this.form);

      this.$alert(this.$t('users.success.add'));
    } catch (e) {
      this.$alert(this.$t('users.errors.add'), AlertType.Error);
    }
  }

  private async updateUserPermissions(): Promise<void> {
    try {
      this.processRolesBeforePush();
      await this.usersPermissionsService.update(this.form);

      this.$alert(this.$t('users.success.update'));
    } catch (e) {
      this.$alert(this.$t('users.errors.update'), AlertType.Error);
    }
  }

  private async deleteUserPermissions(): Promise<boolean> {
    if (
      !(await this.$confirm(
        this.$t('users.confirm.title', [this.form.subjectId])
      ))
    ) {
      return false;
    }

    try {
      await this.usersPermissionsService.remove(this.form);
      this.$alert(this.$t('users.success.delete'));
      return true;
    } catch (e) {
      this.$alert(this.$t('users.errors.delete'), AlertType.Error);
    } finally {
      this.isLoading = false;
    }
  }

  private processRolesAfterGet(): void {
    if (this.form.readPermission) {
      this.role = Role.Reader;
    }

    if (this.form.writePermission) {
      this.role = Role.Contributor;
    }

    if (this.form.managementPermission) {
      this.role = Role.Manager;
    }
  }

  private processRolesBeforePush(): void {
    this.form.readPermission = false;
    this.form.writePermission = false;
    this.form.managementPermission = false;

    if (this.role === Role.Reader || this.role === Role.Contributor || this.role === Role.Manager) {
      this.form.readPermission = true;
    }

    if (this.role === Role.Contributor || this.role === Role.Manager) {
      this.form.writePermission = true;
    }

    if (this.role === Role.Manager) {
      this.form.managementPermission = true;
    }
  }

  private goBack(): void {
    this.$router.push({
      name: 'users-list'
    });
  }
}
</script>

<style lang="scss" scoped>
.users_form {
  &-group {
    padding-left: 0;
  }

  &-select {
    border: 0;
    box-shadow: none;
  }

  .bmd-form-group {
    padding-top: 1.3rem;

    .bmd-label-floating {
      top: 0;
    }
  }
}
</style>
