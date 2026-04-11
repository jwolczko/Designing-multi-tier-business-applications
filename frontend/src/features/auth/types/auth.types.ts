export type LoginRequest = {
  email: string;
  password: string;
};

export type LoginResponse = {
  token: string;
  expiresAtUtc: string;
};

export type AuthState = {
  token: string | null;
  userName: string | null;
  customerId: string | null;
  expiresAtUtc: string | null;
  isAuthenticated: boolean;
};
