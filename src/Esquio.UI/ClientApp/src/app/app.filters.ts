import { Vue } from 'vue-property-decorator';
import { Inject } from 'inversify-props';

import * as s from '~/shared';
import { IFilter } from '~/core';

export class Filters {
    @Inject() dateService: s.IDateService;
    @Inject() translateService: s.ITranslateService;

    public install(): void {
        let filters: any[] = [
            this.dateService,
            this.translateService,
        ];

        filters.forEach((filterService: IFilter) => Vue.filter(filterService.filterName, (...params) => filterService.filterAction(...params)));
    }
}
