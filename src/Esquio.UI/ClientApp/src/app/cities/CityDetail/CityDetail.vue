<template>
<div class="city_detail" v-if="city">
  <header class="city_detail-header">{{city.title}}</header>
  <iframe class="city_detail-map" width="400" height="400" frameborder="0" :src="mapUrl" scrolling="no"></iframe>
</div>
</template>

<script lang="ts">
import { Inject } from 'inversify-props';
import { Component, Vue, Prop } from 'vue-property-decorator';

import { City, ICitiesService } from '~/shared';

@Component
export default class extends Vue {
    public name = 'CityDetail';
    public city: City = null;

    @Inject() citiesService: ICitiesService;

    @Prop() id: number;

    get mapUrl(): string {
        return `https://www.bing.com/maps/embed?h=400&w=400&cp=${this.city.centroid[0]}~${this.city.centroid[1]}&lvl=12&typ=s&sty=r&src=SHELL&FORM=MBEDV8`;
    }

    public async created(): Promise<void> {
        this.city = await this.citiesService.getById(this.id);
    }
}
</script>
<style lang="scss" scoped>
.city_detail {
  color: get-color(basic, bright);
  font-size: get-font-size(l);

  &-header {
    color: get-color(secondary);
    margin-bottom: 1rem;
  }

  &-icon {
    font-size: 3rem;
    margin-right: 1rem;
  }

  &-map {
    border-radius: 50%;
  }
}
</style>