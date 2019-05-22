import { injectable } from 'inversify-props';

import { City, Weather, WeatherForecast } from '~/shared';
import { IWeatherService } from '.';

@injectable()
export class WeatherService implements IWeatherService {
    public weathers: Weather[];

    private toCelsius(faren: number): number {
        return parseFloat(((faren - 32) * 5 / 9).toFixed(2));
    }

    public async get(city: City): Promise<Weather> {
        let response = await fetch(`https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22${encodeURIComponent(city.title)}%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys`);
        let result = await response.json();
        let channel = result.query.results.channel;

        // Map model
        let weather: Weather = {
            astronomy: channel.astronomy,
            item: {
                forecast: channel.item.forecast.map(x => {
                    return <WeatherForecast>{
                        date: new Date(x.date),
                        high: this.toCelsius(parseFloat(x.high)),
                        low: this.toCelsius(parseFloat(x.low)),
                        code: parseInt(x.code, 10),
                        text: x.text
                    };
                })
            }
        };

        return weather;
    }
}
