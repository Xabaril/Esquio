import { injectable } from 'inversify-props';
import Vue from 'vue';
import VueI18n from 'vue-i18n';

import { IFilter } from '~/core';
import { ITranslateService } from '.';

@injectable()
export class TranslateService implements ITranslateService, IFilter {
    public filterName = 't';
    public i18n: VueI18n;

    constructor() {
        Vue.use(VueI18n);

        let messages = {
            en: require('../../../locale/en.locale.json'),
            es: require('../../../locale/es.locale.json')
        };

        this.i18n = new VueI18n({
            locale: 'en',
            messages
        });
    }

    public filterAction(text: string, ...keys: string[]): VueI18n.TranslateResult {
        return this.get(text, ...keys);
    }

    public get(text: string, ...keys: string[]): VueI18n.TranslateResult {
        return this.i18n.t(text, keys);
    }
}
