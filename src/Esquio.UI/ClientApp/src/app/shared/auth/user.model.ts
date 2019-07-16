interface UserProfile {
  name: string;
}

export interface User {
  access_token: string;
  id_token: string;
  scope: string;
  token_type: string;
  profile: UserProfile;
}
