export interface AuditItem {
  productName: string;
  featureName: string;
  oldValues: string; // JSON
  newValues: string; // JSON
}
