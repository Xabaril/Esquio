import { BaseSeed } from '.';
import { City } from '~/shared';

export class CitiesSeed extends BaseSeed {
    constructor() {
        let data: City[] = [
            {
                title: 'London',
                location_type: 'City',
                woeid: 44418,
                centroid: [0, 0],
                weather: null
            },

            {
                title: 'Rome',
                location_type: 'City',
                woeid: 721943,
                centroid: [0, 0],
                weather: null
            },

            {
                title: 'Berlin',
                location_type: 'City',
                woeid: 638242,
                centroid: [0, 0],
                weather: null
            },

            {
                title: 'Washington DC',
                location_type: 'City',
                woeid: 2514815,
                centroid: [0, 0],
                weather: null
            },

            {
                title: 'Lima',
                location_type: 'City',
                woeid: 418440,
                centroid: [0, 0],
                weather: null
            }
        ];

        super('cities', data);
    }
}