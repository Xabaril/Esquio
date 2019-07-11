<template>
  <div
    class="floating"
    :class="{'floating--top': isTop, 'is-disabled': disabled}"
    @click="onClick"
  >
    <router-link :to="to">
      <button
        type="button"
        class="floating-button btn bmd-btn-fab"
        :class="`btn-${cssModifier}`"
        title="Example text"
      >
        <div class="floating-content">
          <i class="floating-icon material-icons">{{icon}}</i>
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
  public cssModifier: FloatingModifier;

  @Prop({ required: true }) text: string;
  @Prop({ default: () => ({}) }) to: RouteConfig;
  @Prop({ default: FloatingIcon.Add }) icon: FloatingIcon;
  @Prop({ default: false }) isTop: boolean;
  @Prop({ default: false }) disabled: boolean;
  @Prop({ default: FloatingModifier.Primary }) modifier: FloatingModifier;

  public created() {
    this.cssModifier = this.modifier;

    if (this.isTop) {
      this.cssModifier = FloatingModifier.Secondary;
    }
  }

  public onClick(): void {
    this.$emit('click');
  }
}
</script>

<style lang="scss" scoped>
.floating {
  $margin: 5vh;
  $top-height: 2.15rem;

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

  &-icon {
    font-size: get-font-size(xl);
  }

  &-content {
    align-content: center;
    align-items: center;
    color: get-color(basic, brightest);
    display: flex;
    font-size: get-font-size(m);
    justify-content: center;
  }

  &-text {
    display: inline-flex;
    padding-left: .5rem;
  }

  &--top {
    bottom: initial;
    position: absolute;
    right: 0;
    top: -$top-height;
  }

  &--top &-button {
    $small-size: 1.75rem;

    height: $small-size;
    margin: 0;
    min-width: $small-size;
    width: $small-size;

    @media screen and (min-width: get-media(xs)) {
      height: $top-height;
      border-radius: 2rem !important;
      min-width: 3.5rem;
      padding: .25rem .75rem !important;
      width: auto !important;
    }
  }

  &--top &-content {
    font-size: get-font-size(s);
  }

  &--top &-icon {
    font-size: get-font-size(l);
  }

  &--top &-text {
    @media screen and (min-width: get-media(xs)) {
      display: inline-flex !important;
    }
  }
}
</style>
