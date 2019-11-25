<template>
  <div class="percentage_parameter col-12" data-testid="percentage-parameter">
    <div class="percentage_parameter-container">
      <div class="percentage_parameter-slider">
        <vue-slider v-model="value" :min="0" :max="100" :tooltip-formatter="formatter"></vue-slider>
      </div>
      <span class="percentage_parameter-value">{{value}}%</span>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import VueSlider from 'vue-slider-component';

@Component({
  components: {
    VueSlider
  }
})
export default class extends Vue {
  public name = 'PercentageParameter';
  public value = 0;
  private oldValue = 0;
  private isEmitting = false;
  private t: number = null;

  @Prop({ required: true }) options: any;

  public created(): void {
    this.value = Number(this.options.value);
    this.oldValue = this.value;
  }

  public formatter(val) {
    return val + '%';
  }

  @Watch('value')
  onChangeValue(nextValue, prevValue) {
    if (this.isEmitting) {
      clearTimeout(this.t);
    }

    if (this.oldValue === this.value) {
      return;
    }

    this.isEmitting = true;

    this.t = setTimeout(() => {
      this.$emit('change', this.value);
      this.oldValue = this.value;
      this.isEmitting = false;
    }, 500);
  }
}
</script>

<style lang="scss" scoped>
.percentage_parameter {
  &-container {
    display: block;
    margin: 0 auto;
    width: 80%;
  }

  &-slider {
    display: inline-block;
    width: 80%;
  }

  &-value {
    color: get-color(secondary);
    display: inline-block;
    font-size: get-font-size(l);
    font-weight: get-font-weight(bold);
    margin-left: 5%;
    transform: translateY(-.1rem);
    width: 15%;
  }
}
</style>
