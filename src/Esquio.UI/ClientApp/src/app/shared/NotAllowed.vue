<template>
  <section class="not_allowed container u-container-medium">
    <h1 class="not_allowed-title">{{$t('not_allowed.title')}}</h1>

    <div class="container">
      <div class="row">
        <div class="col d-flex justify-content-end">
          <img
            class="not_allowed-image"
            src="~/assets/images/esquio01.png"
          />
        </div>
        <div class="col">
          <div class="not_allowed-symbol">¿?</div>
        </div>
      </div>
    </div>

    <div class="not_allowed-actions">
      <router-link to="/logout">
        <button
          type="button"
          class="btn btn-lg btn-raised btn-primary"
        >
          {{$t('not_allowed.action')}}
        </button>
      </router-link>
    </div>
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'NotAllowed';
}
</script>

<style lang="scss" scoped>
.not_allowed {
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
    width: 20rem;
  }

  &-symbol {
    align-items: center;
    display: flex;
    font-size: 10rem;
    height: 100%;
    justify-content: flex-start;
    opacity: .75;
    padding-left: 2rem;
  }
}
</style>
