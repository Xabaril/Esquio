<template>
  <div class="parameter">
    <component :is="selectedComponent"></component>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { ParameterDetailType } from './parameter-detail-type.enum';
import { default as PercentageParameter } from './PercentageParameter.vue';

@Component
export default class extends Vue {
  public name = 'Parameter';
  private components = {
    EsquioPercentage: PercentageParameter
  };

  @Prop({ required: true, type: String}) type: ParameterDetailType;

  public get selectedComponent(): Vue {
    const [type] = Object.entries(ParameterDetailType).find(([key, value]) => {
      return value === this.type;
    });

    if (!type) {
      throw new Error(`The component ${this.type} is not valid component.`);
    }

    const selected = this.components[type];

    if (!selected) {
      throw new Error(`The component ${type} component is not registered.`);
    }

    return selected;
  }
}
</script>
