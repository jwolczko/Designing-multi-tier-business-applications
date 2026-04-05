import type { LoginRequest, LoginResponse } from '../types/auth.types';

export async function loginRequest(payload: LoginRequest): Promise<LoginResponse> {
  await new Promise((resolve) => setTimeout(resolve, 500));

  return {
    token: 'demo-token',
    userName: 'Imię i Nazwisko',
    customerId: 'customer-1',
  };
}

export async function logoutRequest(): Promise<void> {
  await new Promise((resolve) => setTimeout(resolve, 200));
}