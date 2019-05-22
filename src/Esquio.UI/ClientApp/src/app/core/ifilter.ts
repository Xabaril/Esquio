export interface IFilter {
    filterName: string;
    filterAction(...params): any;
}