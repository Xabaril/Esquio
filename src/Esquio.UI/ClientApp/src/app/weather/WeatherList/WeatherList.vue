<template>
<div class="weather_list">
  <ul class="weather_list-cities">
    <li class="weather_list-city" v-for="(city, index) in cities" @click="goToCity(city.woeid)" :key="index">
      <span class="weather_list-title">{{city.title}}</span>
      <div v-if="city.weather">
        <div v-if="city.weather.astronomy">
          <span class="weather_list-item">{{'weather.sunrise' | t }}: {{city.weather.astronomy.sunrise}}</span>
          <span class="weather_list-item">{{'weather.sunset' | t }}: {{city.weather.astronomy.sunset}}</span>
        </div>
        <div v-if="city.weather.item">
          <span>{{city.weather.item.forecast[0].date | date}} (Today):</span>
          <span class="weather_list-item">{{city.weather.item.forecast[0].low}}ºC / {{city.weather.item.forecast[0].high}}ºC</span>
          <div>
            <span class="weather_list-item">
              <i :class="['weather_list-icon wi', getWeatherIconClass(city.weather.item.forecast[0].code)]"></i>
            </span>
            <span class="weather_list-item">{{city.weather.item.forecast[0].text}}</span>
          </div>
        </div>
      </div>
    </li>
  </ul>
</div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

import { Inject } from 'inversify-props';
import { ICitiesService, City, WeatherCodes, WeatherIcons } from '~/shared';
import { IWeatherService } from '~/weather/shared';

@Component
export default class extends Vue {
    public name = 'WeatherList';
    public cities: City[] = [];

    @Inject() citiesService: ICitiesService;
    @Inject() weatherService: IWeatherService;

    public async created(): Promise<void> {
        this.cities = await this.citiesService.get();
        this.cities.forEach(async city => {
            city.weather = await this.weatherService.get(city);
        });
    }

    public goToCity(id: string): void {
        this.$router.push({
            name: 'weather-detail',
            params: {
                id: id
            }
        });
    }

    // Refactor
    public getWeatherIconClass(code: number): string {
        return WeatherIcons[code] || WeatherIcons[WeatherCodes.NotAvailable];
    }
}
</script>

<style lang="scss" scoped>
.weather_list {
  color: get-color(basic, bright);
  font-size: get-font-size(l);

  &-city {
    background-color: get-color(basic, bright);
    border-radius: 5px;
    cursor: pointer;
    padding: 1vw 3vw;
    transition: background-color get-time();

    &:hover {
      background-color: get-color(basic, bright);
    }
  }

  &-title {
    font-size: get-font-size(xl);
    font-weight: get-font-weight(bold);
  }

  &-item {
    display: inline-block;
    padding: 1rem 0;
    vertical-align: middle;
  }

  &-icon {
    font-size: 3rem;
    margin-right: 1rem;
    perspective: 200;
    perspective-origin: 0 0;
    transform-origin: 50% 50%;
    transition: all get-time(slow);
  }

  &-city:hover &-icon {
    transform: rotateY(180deg);
  }
}
</style>