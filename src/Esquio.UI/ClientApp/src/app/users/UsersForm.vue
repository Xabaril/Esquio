<template>
  <section class="products_form container u-container-medium pl-0 pr-0">
    <div class="row">
      <h1>{{$t('users.detail')}}</h1>
    </div>
    <form class="row">
      <input-text
        class="products_form-group form-group col-md-6"
        v-model="form.subjectId"
        id="user_name"
        :label="$t('users.fields.subject')"
        validators="required|min:1"
        :help-label="$t('users.placeholders.subjectHelp')"
      />

      // Checkboxes
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
  public form: UserPermissions = { id: null, subjectId: null, isAuthorized: false, managementPermission: false, readPermission: false, writePermission: false };

  @Inject() usersPermissionsService: IUsersPermissionsService;

  @Prop({ type: [String, Number]}) id: string;

  get isEditing(): boolean {
    return !!this.id;
  }

  get areActionsDisabled(): boolean {
    return (
      !this.form.subjectId ||
      this.$validator.errors.count() > 0
    );
  }

  public async created(): Promise<void> {
    if (!this.isEditing) {
      return;
    }

    // await this.getUserPermissions();
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

  private async addUserPermissions(): Promise<void> {
    try {
      await this.usersPermissionsService.add(this.form);

      this.$alert(this.$t('users.success.add'));
    } catch (e) {
      this.$alert(this.$t('users.errors.add'), AlertType.Error);
    }
  }

  private async updateUserPermissions(): Promise<void> {
    try {
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

  private goBack(): void {
    this.$router.push({
      name: 'users-list'
    });
  }
}
</script>

<style lang="scss" scoped>
.products_form {
  &-group {
    padding-left: 0;
  }
}
</style>
