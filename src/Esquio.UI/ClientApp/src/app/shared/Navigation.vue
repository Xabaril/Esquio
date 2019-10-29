<template>
<header class="navigation">
  <div class="container navigation-container">
    <router-link to="/" class="navigation-title">
      {{$t('common.title')}}
    </router-link>
    <div class="navigation-items">
      <!-- <router-link class="navigation-link navigation-link--home" :to="{ name: 'home'}" active-class="active">{{$t('common.menu.home')}}</router-link> -->
      <router-link v-if="breadcrumb.length > 0" class="navigation-link navigation-link--breadcrumb" :to="{ name: 'products-list'}" active-class="active">{{$t('common.menu.products')}}</router-link>

      <router-link v-for="page in breadcrumb" :key="page.name" class="navigation-link navigation-link--breadcrumb" :to="{ name: page.name, params: {id: page.id, productId: page.productId}}" active-class="active">{{$t(`breadcrumb.${page.name}`, [page.id])}}</router-link>
    </div>
    <div v-if="user" class="navigation-profile">
      <b-dropdown :text="`${user.profile.name} ${$t(user.roleName)}`" variant="outline-light">
      <router-link  to="/logout" tag="b-dropdown-item">{{$t('submenu.logout')}}</router-link>
      <router-link v-if="$can($constants.AbilityAction.Manage, $constants.AbilitySubject.Permission)" to="/users" tag="b-dropdown-item">{{$t('submenu.users')}}</router-link>

      <b-dropdown-item href="#" @click="onClickGenerateToken">{{$t('submenu.token')}}</b-dropdown-item>
    </b-dropdown>
    </div>
  </div>
</header>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import { IAuthService } from './auth';
import { User, AbilityAction, AbilitySubject } from './user';
import { Inject } from 'inversify-props';
import { BreadCrumbItem, generateBreadcrumb } from './breadcrumb';
import { Route } from 'vue-router';
import { ITokensService } from './tokens';
import { AlertType } from '~/core';
import { nextTick } from '~/core/helpers';

@Component
export default class extends Vue {
  public name = 'Navigation';
  public user: User = null;
  public breadcrumb: BreadCrumbItem[] = [];

  @Inject() authService: IAuthService;
  @Inject() tokensService: ITokensService;

  // In the future this will be it owns component
  public async onClickGenerateToken(): Promise<void> {
    try {
      const response = await this.tokensService.generate();
      // Super fast hack, because this is temporal :)
      const $copy = document.createElement('textarea');
      $copy.classList.add('is-invisible');
      $copy.value = response.apiKey;
      document.querySelector('body').appendChild($copy);
      await nextTick(100);
      $copy.select();
      document.execCommand('copy');
      $copy.remove();

      this.$alert(this.$t('tokens.success'));
    } catch (e) {
      this.$alert(this.$t('tokens.error'), AlertType.Error);
    }
  }

  private configureAbilities(): void {
    if (!this.$ability || !this.authService.userAbility || this.$ability.rules.length > 0) {
      return;
    }

    this.$ability.update(this.authService.userAbility.rules);
  }

  @Watch('$route') async onChangeRoute(nextRoute: Route) {
    await this.authService.getUser();
    this.breadcrumb = generateBreadcrumb(nextRoute);
    this.user = this.user || this.authService.user;

    this.configureAbilities();
  }
}
</script>

<style lang="scss" scoped>
.navigation {
  $height: $navigation-height;

  background-color: get-color(primary);
  height: $height;
  width: 100%;

  &-container {
    align-items: center;
    display: flex;
    height: 100%;
    justify-content: space-between;
  }

  &-title {
    color: get-color(basic, brightest);
    font-size: get-font-size(xl);

    @media screen and (min-width: get-media(xs)) {
      position: absolute;
    }
  }

  &-items {
    @media screen and (min-width: get-media(xs)) {
      margin: 0 auto;
    }
  }

  &-link {
    align-items: center;
    border-bottom: 2px solid transparent;
    color: get-color(basic, brightest);
    display: inline-flex;
    height: $height * .35;
    margin-right: 1rem;
    text-decoration: none !important;
    transition: border-bottom-color get-time(normal);

    &:hover {
      border-color: rgba(get-color(primary, brightest), .5);
    }

    &--breadcrumb {
      margin-right: .5rem;

      &:after {
        border-bottom: 5px solid get-color(primary);
        border-top: 5px solid get-color(primary);
        content: '/';
        padding-left: .5rem;
      }

      &:last-of-type {
        pointer-events: none;

        &:after {
          content: '';
        }
      }
    }
  }
}
</style>
