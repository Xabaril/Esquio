import localforage from 'localforage';

export class BaseSeed {
    private prefix = 'seed-';

    constructor(
        protected key: string,
        protected data: any[]
    ) {}

    public async initialize() {
        let exist = await localforage.getItem(this.prefix + this.key);
        if (exist) {
            return;
        }

        return localforage.setItem(this.prefix + this.key, this.data);
    }
}