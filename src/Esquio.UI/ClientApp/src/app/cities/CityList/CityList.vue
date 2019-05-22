<template>
<section class="city_list">
  <city-add></city-add>

  <loading v-if="!cities" />

  <ul class="city_list-cities" v-if="cities">
    <li class="city_list-city content-end" v-for="(city, index) in cities" :key="index">
      <span class="city_list-title">{{city.title}}</span>
      <router-link class="city_list-view badges-list-item" :to="{ name: 'city-detail', params: { id: city.woeid }}" title="view">➤</router-link>
      <span class="city_list-close badges-list-item" @click="remove(city.woeid)" title="delete">×</span>
    </li>
  </ul>
</section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';

import { City, ICitiesService, Loading } from '~/shared';
import CityAdd from '../shared/CityAdd.vue';

@Component({
    components: {
        CityAdd,
        Loading,
    }
})
export default class extends Vue {
    public name = 'CityList';
    public cities: City[] = null;

    @Inject() citiesService: ICitiesService;

    public async created(): Promise<void> {
        // Fake example with loading
        setTimeout(async () => {
            this.cities = await this.citiesService.get();
        }, 2000);
    }

    public remove(id: number): void {
        this.citiesService.remove(id);
    }
}
</script>

<style lang="scss" scoped>
.city_list {
  &-title {
    margin-right: auto;
  }

  &-close {
    cursor: pointer;
    margin-left: 1rem;
    transition: color get-time();

    &:hover {
      color: get-color(secondary);
      transition: color get-time();
    }
  }
}
</style>
