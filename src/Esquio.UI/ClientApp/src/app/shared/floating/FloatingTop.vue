<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RouteConfig } from 'vue-router';
import { FloatingIcon } from './floating-icon';
import { FloatingModifier } from './floating-modifier';
import { Floating } from '.';

@Component
export default class extends Floating {
  public name = 'FloatingTop';
  public isTop = true;

}
</script>
