import { Link, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import './LoginPanel.css';

type LoginPanelProps = {
  onOpenSupport: () => void;
  onClose?: () => void;
  isModal?: boolean;
};

export function LoginPanel({ onOpenSupport, onClose, isModal = false }: LoginPanelProps) {
  const navigate = useNavigate();
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = () => {
    if (!login.trim() || !password.trim()) return;
    onClose?.();
    navigate('/dashboard');
  };

  return (
    <div className={`login-panel${isModal ? ' login-panel--modal' : ''}`}>
      <div className="login-panel__form">
          <h1>Logowanie do systemu:</h1>

          <label className="login-panel__label" htmlFor="login">
            Twój login
          </label>
          <input
            id="login"
            className="login-panel__input"
            type="text"
            value={login}
            onChange={(event) => setLogin(event.target.value)}
          />

          <label className="login-panel__label" htmlFor="password">
            Hasło <span className="login-panel__hint"></span>
          </label>
          <input
            id="password"
            className="login-panel__input"
            type="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
          />

          <div className="login-panel__buttons">
            {onClose ? (
              <button className="login-panel__back-btn" type="button" onClick={onClose}>
                WSTECZ
              </button>
            ) : (
              <Link className="login-panel__back-btn" to="/">
                WSTECZ
              </Link>
            )}
            <button className="login-panel__submit-btn" type="button" onClick={handleSubmit}>
              ZALOGUJ
            </button>
          </div>

          <button className="login-panel__support-link" type="button" onClick={onOpenSupport}>
            Problemy z logowaniem
          </button>
      </div>
    </div>
  );
}
