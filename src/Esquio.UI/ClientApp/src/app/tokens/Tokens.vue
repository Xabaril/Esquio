<template>
<section class="tokens">
  <router-view></router-view>
</section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'Tokens';
}
</script>
