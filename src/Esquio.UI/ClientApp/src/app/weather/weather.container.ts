import { container } from 'inversify-props';
import { IWeatherService, WeatherService } from './shared';

export default () => {
    container.addSingleton<IWeatherService>(WeatherService);
};
