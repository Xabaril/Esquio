<template>
<div class="not_allowed">You're not allowed to open this page, please <router-link to="/logout">choose other account</router-link></div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'NotFound';
}
</script>
