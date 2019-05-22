import localforage from 'localforage';
import { injectable } from 'inversify-props';

import { City, ICitiesService } from '.';

@injectable()
export class CitiesService implements ICitiesService {
    private cities: City[];

    public async get(): Promise<City[]> {
        if (this.cities) {
            return Promise.resolve(this.cities);
        }

        this.cities = await localforage.getItem('seed-cities') as City[];

        return Promise.resolve(this.cities);
    }

    public async getById(id: number): Promise<City> {
        return this.cities.find(city => city.woeid === +id);
    }

    public async search(name: string): Promise<City> {
        let response = await fetch(`https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20geo.places(1)%20where%20text%3D%22${encodeURIComponent(name)}%22&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys`);
        let result = await response.json();
        let place = result.query.results.place;
        let locality = place.locality1;

        if (!locality) {
            return;
        }

        let city: City = {
            title: locality.content,
            location_type: locality.type,
            woeid: parseInt(locality.woeid, 10),
            centroid: [parseFloat(place.centroid.latitude), parseFloat(place.centroid.longitude)],
            weather: null
        };

        if (!this.cities.find(x => x.title.toLowerCase() === city.title.toLowerCase())) {
            this.cities.push(city);
            await localforage.setItem('seed-cities', this.cities);
        }

        return city;
    }

    public async remove(id: number): Promise<City[]> {
        let index = this.cities.findIndex(city => city.woeid === id);
        this.cities.splice(index, 1);
        return localforage.setItem('seed-cities', this.cities);
    }
}
