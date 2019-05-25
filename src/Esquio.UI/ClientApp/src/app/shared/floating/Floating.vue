<template>
  <div class="floating">
    <router-link :to="to">
      <button
      type="button"
      class="floating-button btn bmd-btn-fab"
      :class="`btn-${modifier}`"
      title="Example text"
    >
      <div class="floating-content">
        <i class="material-icons">{{icon}}</i>
        <span class="floating-text is-hidden@s">{{text}}</span>
      </div>
    </button>
  </router-link>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RouteConfig } from 'vue-router';
import { FloatingIcon } from './floating-icon';
import { FloatingModifier } from './floating-modifier';

@Component
export default class extends Vue {
  public name = 'Floating';

  @Prop({ required: true }) text: string;
  @Prop({ default: () => ({})}) to: RouteConfig;
  @Prop({ default: FloatingIcon.Add }) icon: FloatingIcon;
  @Prop({ default: FloatingModifier.Secondary }) modifier: FloatingModifier;
}
</script>

<style lang="scss" scoped>
.floating {
  $margin: 5vh;

  position: fixed;
  bottom: $margin;
  right: $margin;

  &-button {
    @media screen and (min-width: get-media(s)) {
      border-radius: 2rem !important;
      padding: 1rem !important;
      width: auto !important;
    }
  }

  &-content {
    align-content: center;
    align-items: center;
    color: get-color(basic, brightest);
    display: flex;
    font-size: get-font-size(m);
    justify-content: center;

    @media screen and (min-width: get-media(m)) {
      transform: translateY(-.15rem);
    }
  }

  &-text {
    display: inline-flex;
    padding-left: .5rem;
  }
}
</style>
