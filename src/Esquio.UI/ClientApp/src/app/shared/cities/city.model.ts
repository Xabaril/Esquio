import { Weather } from '~/shared';

export interface City {
    title: string;
    location_type: string;
    woeid: number;
    centroid: [number, number];
    weather: Weather;
}