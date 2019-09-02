export interface ToggleTypesInfo {
  assembly: string;
  type: string;
  description: string;
}

export interface ToggleTypes {
  scannedAssemblies: number;
  toggles: ToggleTypesInfo[];
}
