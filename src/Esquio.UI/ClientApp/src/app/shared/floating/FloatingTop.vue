<script lang="ts">
import { Component } from 'vue-property-decorator';
import { Floating } from '.';

@Component
export default class extends Floating {
  public name = 'FloatingTop';
  public isTop = true;

}
</script>
