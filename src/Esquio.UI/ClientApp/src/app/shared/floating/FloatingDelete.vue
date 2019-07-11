<script lang="ts">
import { Component } from 'vue-property-decorator';
import { Floating } from '.';
import { FloatingModifier } from './floating-modifier';
import { FloatingIcon } from './floating-icon';

@Component
export default class extends Floating {
  public name = 'FloatingDelete';
  public icon = FloatingIcon.Delete;
  public modifier = FloatingModifier.Warning;
}
</script>
