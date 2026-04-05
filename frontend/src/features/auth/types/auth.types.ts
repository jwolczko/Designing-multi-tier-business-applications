export type LoginRequest = {
  login: string;
  password: string;
};

export type LoginResponse = {
  token: string;
  userName: string;
  customerId: string;
};

export type AuthState = {
  token: string | null;
  userName: string | null;
  customerId: string | null;
  isAuthenticated: boolean;
};