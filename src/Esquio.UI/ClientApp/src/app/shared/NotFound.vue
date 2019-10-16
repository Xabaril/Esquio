<template>
  <section class="not_found container u-container-medium">
    <h1 class="not_found-title">{{$t('not_found.title')}}</h1>

    <div class="container">
      <div class="row">
        <div class="col d-flex justify-content-center">
          <img
            class="not_found-image"
            src="~/assets/images/esquio02.png"
          />
        </div>
      </div>
    </div>

    <div class="not_found-actions">
      <router-link to="/">
        <button
          type="button"
          class="btn btn-lg btn-raised btn-primary"
        >
          {{$t('not_found.action')}}
        </button>
      </router-link>
    </div>
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'NotFound';
}
</script>

<style lang="scss" scoped>
.not_found {
  &-title {
    margin-top: 2rem;
    text-align: center;
  }

  &-actions {
    margin: 0 auto;
    text-align: center;
  }

  &-image {
    margin: 2rem 0;
    transform: translateX(-2.5rem);
    width: 20rem;
  }
}
</style>
