<template>
<div class="navigation">
  <div class="container navigation-container">
    <div class="navigation-title">
      {{$t('common.title')}}
    </div>
    <div>
      <router-link class="navigation-link navigation-link--home" :to="{ name: 'home'}" active-class="active">{{$t('common.menu.home')}}</router-link>
      <router-link class="navigation-link" :to="{ name: 'products-list'}" active-class="active">{{$t('common.menu.products')}}</router-link>
    </div>
  </div>
</div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'Navigation';
}
</script>

<style lang="scss" scoped>
.navigation {
  $height: 5rem;

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
