import { Time } from './time.model';

export interface IDateService {
  dateTimeToString(date: Date, time?: Time): string;
  stringToDateTime(rawDate: string): [Date, Time];
  fancyFormatDateTime(rawDate: Date | string): string;
}
