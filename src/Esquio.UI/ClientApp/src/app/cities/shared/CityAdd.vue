<template>
  <section class="city_add">
    <div class="form-collapse">
      <div class="input item">
        <input class="city_add-input" type="text" v-model="cityName">
      </div>
      <button class="city_add-button item button button-primary" @click="search">{{'cities.search' | t }}</button>
    </div>
  </section>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';

import { City, ICitiesService } from '~/shared';

@Component
export default class extends Vue {
    public name = 'CityAdd';
    public city: City = null;
    public cityName = '';

    @Inject() citiesService: ICitiesService;

    public async search(): Promise<void> {
        if (!this.cityName) {
            return;
        }

        this.city = await this.citiesService.search(this.cityName);
    }

    public testableMethods(num: number) {
      return num + 1;
    }
}
</script>

<style lang="scss" scoped>
.city_add {}
</style>
