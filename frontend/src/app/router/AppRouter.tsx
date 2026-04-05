import { Routes, Route, Navigate } from 'react-router-dom';
import { PublicLayout } from '../../layouts/PublicLayout';
import { DashboardLayout } from '../../layouts/DashboardLayout';
import { LandingPage } from '../../views/LandingPage/LandingPage';
import { LoginPage } from '../../views/LoginPage/LoginPage';
import { DashboardPage } from '../../views/DashboardPage/DashboardPage';

export function AppRouter() {
  return (
    <Routes>
      <Route element={<PublicLayout />}>
        <Route path="/" element={<LandingPage />} />
        <Route path="/login" element={<LoginPage />} />
      </Route>

      <Route element={<DashboardLayout />}>
        <Route path="/dashboard" element={<DashboardPage />} />
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}