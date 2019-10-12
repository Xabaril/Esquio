<template>
<section class="user_permissions_input">

</section>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { UserPermissions } from '~/shared';

@Component
export default class extends Vue {
  public name = 'UserPermissionsInput';

  @Prop({ type: Object }) permissions: UserPermissions;
}
</script>
