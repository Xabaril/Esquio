<template>
  <div class="app">
    <Navigation />

    <div class="app-content">
      <router-view></router-view>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Navigation } from '~/shared';

@Component({
  components: {
    Navigation
  }
})
export default class extends Vue {
  public name = 'App';
}
</script>

<style lang="scss" scoped>
.app {
  &-content {
    margin-top: 1rem;
  }
}
</style>
