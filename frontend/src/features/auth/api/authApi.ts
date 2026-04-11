import type { LoginRequest, LoginResponse } from '../types/auth.types';
import { apiRequest } from '../../../app/apiClient';

type LoginApiResponse = {
  token: string;
  expiresAtUtc: string;
};

export async function loginRequest(payload: LoginRequest): Promise<LoginResponse> {
  const response = await apiRequest<LoginApiResponse>('/api/customers/login', {
    method: 'POST',
    body: JSON.stringify({
      email: payload.email,
      password: payload.password,
    }),
  });

  return {
    token: response.token,
    expiresAtUtc: response.expiresAtUtc,
  };
}

export async function logoutRequest(): Promise<void> {
  return Promise.resolve();
}
