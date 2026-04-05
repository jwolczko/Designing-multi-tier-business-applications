import type { DashboardData } from '../types/dashboard.types';

export async function getDashboardData(): Promise<DashboardData> {
  await new Promise((resolve) => setTimeout(resolve, 400));

  return {
    totalBalance: '10 333,70 PLN',
    accounts: 3,
    events: 10,
  };
}