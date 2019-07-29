<template>
  <div class="custom_switch" :class="{'is-checked': realValue}">
    <input class="is-hidden" type="checkbox" v-model="realValue">
    <span class="custom_switch-button" :class="{'is-active': realValue}" @click="toggle">{{$t('common.switch.on')}}</span>
    <span class="custom_switch-button" :class="{'is-active': !realValue}" @click="toggle">{{$t('common.switch.off')}}</span>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component
export default class extends Vue {
  public name = 'CustomSwitch';

  @Prop({ default: '', type: String }) label: string;
  @Prop({ required: true, type: Boolean }) value: boolean;

  public get realValue(): boolean {
    return this.value;
  }

  public set realValue(value: boolean) {
    this.$emit('input', value);
    this.$emit('change', value);
  }

  public toggle(): void {
    this.realValue = !this.realValue;
  }
}
</script>

<style lang="scss" scoped>
.custom_switch {
  $size: get-font-size(m);

  border: 1px solid rgba(get-color(primary, darkest), .75);
  border-radius: $size;
  display: inline-flex;
  line-height: $size;
  justify-content: space-around;
  padding: .15rem;
  position: relative;
  width: 9rem;

  &-button {
    color: get-color(primary);
    font-size: get-font-size(s);
    position: relative;
    text-align: center;
    transition: color get-time(slow);
    width: 50%;
    z-index: 1;

    &.is-active {
      color: get-color(basic, brightest);
    }
  }

  &::before {
    $padding: .1rem;

    content: '';
    background-color: get-color(primary);
    border-radius: $size;
    height: $size;
    left: $padding;
    position: absolute;
    top: $padding * 1.5;
    transform: translateX(100%);
    transition: transform get-time(normal);
    width: calc(50% - #{$padding});
    z-index: 0;
  }

  &.is-checked::before {
    transform: translateX(0);
  }
}
</style>
