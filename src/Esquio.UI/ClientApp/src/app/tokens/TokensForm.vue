<template>
  <section class="tokens_form container u-container-medium pl-0 pr-0">
    <div class="row">
      <h1>{{$t('tokens.detail')}}</h1>
    </div>
    <form class="row">
      <input-text
        class="tokens_form-group form-group col-md-6"
        v-model="form.name"
        id="user_name"
        :label="$t('tokens.fields.name')"
        validators="required|min:5|regex:^[a-zA-Z]+(?:-[a-zA-Z0-9]+)*$"
        :help-label="$t('tokens.placeholders.nameHelp')"
      />

      <div class="tokens_form-group form-group col-md-6">
        <label for="role" class="bmd-label-floating ml-3">
          {{$t('tokens.placeholders.validTo')}}
        </label>
        <DateParameter :options="dateOptions" @change="onChangeDate"/>
      </div>
    </form>

    <div class="row" v-if="result">
      <b-alert show>{{$t('tokens.copy')}}</b-alert>
      <b-alert variant="success" show>
        {{result}}
        <i class="tokens_form-copy material-icons" @click="onClickCopy">file_copy</i>
      </b-alert>
    </div>

    <FloatingContainer>
      <FloatingSave
        :text="$t('tokens.actions.save')"
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
  UserPermissions,
  IDateService,
  DateParameter
} from '~/shared';
import { AlertType } from '~/core';
import { Token } from './token.model';
import { ITokensService } from './itokens.service';
import copy from 'copy-to-clipboard';

enum Role {
  Manager,
  Contributor,
  Reader
}

@Component({
  components: {
    FloatingSave,
    FloatingTop,
    FloatingContainer,
    InputText,
    DateParameter
  }
})
export default class extends Vue {
  public name = 'UsersForm';
  public isLoading = false;
  public result: string = null;

  public form: Token = {
    name: null,
    validTo: null
  };

  public dateOptions = {
    form: this.form
  };

  @Inject() dateService: IDateService;
  @Inject() tokensService: ITokensService;

  get areActionsDisabled(): boolean {
    return !this.form.name || !this.form.validTo || this.$validator.errors.count() > 0;
  }

  public async onClickSave(): Promise<void> {
    if (this.$validator.errors.count() > 1) {
      return;
    }

    await this.addToken();
  }

  public onChangeDate(date: Date): void {
    this.form.validTo = new Date(date);
  }

  public onClickCopy(): void {
    this.copyToken();
  }

  private async addToken(): Promise<void> {
    try {
      const response = await this.tokensService.add(this.form);
      this.result = response.key;

      this.form = {
        name: null,
        validTo: new Date()
      };

      this.dateOptions = {
        form: this.form
      };

      this.$emit('add');
      this.$validator.reset();

      this.$alert(this.$t('tokens.success.add'));
    } catch (e) {
      this.$alert(this.$t('tokens.errors.add'), AlertType.Error);
    }
  }

  private copyToken(): void {
      copy(this.result);
      this.$alert(this.$t('tokens.success.copy'));
  }
}
</script>

<style lang="scss" scoped>
.tokens_form {
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

  &-copy {
    cursor: pointer;
    font-size: 1.4rem;
    margin-left: .5rem;
    transform: translateY(.25rem);
  }
}
</style>
