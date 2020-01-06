import { injectable } from 'inversify-props';
import { IDateService } from './idate.service';
import { Time } from './time.model';

@injectable()
export class DateService implements IDateService {

  public dateTimeToString(date: Date, time?: Time): string {
    return `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}` + (time ? ` ${time.HH}:${time.mm}:${time.ss}`: '');
  }

  public stringToDateTime(rawDate: string): [Date, Time] {
    let date = new Date(rawDate);
    let time: Time;

    if (isNaN(date.getTime())) {
      date = new Date();
      time = {
        HH: '0',
        mm: '0',
        ss: '0'
      };

      return;
    }

    const hh = date.getHours() + '';
    const mm = date.getMinutes() + '';
    const ss = date.getSeconds() + '';

    time = {
      HH: hh.length > 1 ? hh : `0${hh}`,
      mm: mm.length > 1 ? mm : `0${mm}`,
      ss: ss.length > 1 ? ss : `0${ss}`
    };

    return [date, time];
  }

  public fancyFormatDateTime(rawDate: Date | string): string {
    const [date, time] = this.stringToDateTime(rawDate.toString());
    debugger;

    return this.dateTimeToString(date, time);
  }
}
