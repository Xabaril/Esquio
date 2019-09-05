<template>
<div class="navigation">
  <div class="container navigation-container">
    <div class="navigation-title">
      {{$t('common.title')}}
    </div>
    <div class="navigation-items">
      <!-- <router-link class="navigation-link navigation-link--home" :to="{ name: 'home'}" active-class="active">{{$t('common.menu.home')}}</router-link> -->
      <router-link class="navigation-link" :to="{ name: 'products-list'}" active-class="active">{{$t('common.menu.products')}}</router-link>

      <router-link v-for="page in breadcrumb" :key="page.name" class="navigation-link" :to="{ name: page.name, props: {id: page.id}}" active-class="active">{{page.name + page.id}}</router-link>
    </div>
    <div v-if="user" class="navigation-profile">
      <router-link class="navigation-link navigation-link--home" to="/logout">{{user.profile.name}}</router-link>
    </div>
  </div>
</div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import { IAuthService, User } from './auth';
import { Inject } from 'inversify-props';
import { BreadCrumbItem, generateBreadcrumb } from './breadcrumb';
import { Route } from 'vue-router';

@Component
export default class extends Vue {
  public name = 'Navigation';
  public user: User = null;
  public breadcrumb: BreadCrumbItem[] = [];

  @Inject() authService: IAuthService;

  @Watch('$route') onChangeRoute(nextRoute: Route) {
    this.breadcrumb = generateBreadcrumb(nextRoute);
    console.log(this.breadcrumb);
    this.user = this.user || this.authService.user;
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
    margin-left: 1rem;
    text-decoration: none !important;
    transition: border-bottom-color get-time(normal);

    &:hover {
      border-bottom-color: rgba(get-color(primary, brighter), .5);
    }

    &.router-link-exact-active,
    &.active:not(.navigation-link--home) {
      border-bottom-color: get-color(primary, brighter);
    }
  }
}
</style>
