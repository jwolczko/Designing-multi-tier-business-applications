import { useState } from 'react';
import { LoginPanel } from '../../components/login/LoginPanel/LoginPanel';
import { LoginSupportModal } from '../../components/login/LoginSupportModal/LoginSupportModal';
import './LoginPage.css';

export function LoginPage() {
  const [isSupportModalOpen, setIsSupportModalOpen] = useState(false);

  return (
    <div className="login-page">
      <LoginPanel onOpenSupport={() => setIsSupportModalOpen(true)} />
      {isSupportModalOpen && <LoginSupportModal onClose={() => setIsSupportModalOpen(false)} />}
    </div>
  );
}