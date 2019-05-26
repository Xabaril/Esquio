export interface Feature {
  id: string;
  name: string;
  description: string;
  enabled: boolean;
  rollout: boolean;
  tags?: any; // in progress
}
