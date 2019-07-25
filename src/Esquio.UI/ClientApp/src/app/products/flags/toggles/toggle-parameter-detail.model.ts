export enum ToggleParameterDetailType {
  EsquioSemicolonList = 'Esquio.SemicolonList',
  EsquioPercentage = 'Esquio.Percentage',
  EsquioDate = 'Esquio.Date',
  EsquioString = 'Esquio.String'
}

export interface ToggleParameterDetail {
  name: number;
  clrType: ToggleParameterDetailType;
  description: string;
}
