import { City } from '.';

export interface ICitiesService {
    get(): Promise<City[]>;
    getById(id: number): Promise<City>;
    search(name: string): Promise<City>;
    remove(id: number): Promise<City[]>;
}