import { injectable } from 'inversify-props';

import { IFilter } from '~/core';
import { IDateService } from '.';

@injectable()
export class DateService implements IDateService, IFilter {
    public filterName = 'date';

    public filterAction(date: Date): string {
        return this.formatDate(date);
    }

    public formatDate(date: Date): string {
        if (!this.isValidDate(date)) {
            return '';
        }

        let day: string | number = date.getDay();
        if (day < 10) {
            day = '0' + day.toString();
        }

        let month: string | number = date.getMonth() + 1;
        if (month < 10) {
            month = '0' + month.toString();
        }

        return `${month}/${day}/${date.getFullYear()}`;
    }

    private isValidDate(date: Date): boolean {
        if (Object.prototype.toString.call(date) === '[object Date]') {
            if (isNaN(date.getTime())) {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            return false;
        }
    }
}
