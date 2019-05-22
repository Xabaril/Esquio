import { City, Weather } from '~/shared';

export interface IWeatherService {
    get(city: City): Promise<Weather>;
}