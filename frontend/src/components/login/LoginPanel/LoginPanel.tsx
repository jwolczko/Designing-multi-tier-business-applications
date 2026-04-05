import { Link, useNavigate } from 'react-router-dom';
import { useState } from 'react';
import './LoginPanel.css';

type LoginPanelProps = {
  onOpenSupport: () => void;
};

export function LoginPanel({ onOpenSupport }: LoginPanelProps) {
  const navigate = useNavigate();
  const [password, setPassword] = useState('');

  const handleSubmit = () => {
    if (!password.trim()) return;
    navigate('/dashboard');
  };

  return (
    <div className="login-panel">
      <div className="login-panel__left">
        <div className="login-panel__form">
          <h1>Logowanie do systemu:</h1>

          <div className="login-panel__login-row">
            <span>Twój login:</span>
            <strong>123456789</strong>
          </div>

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
            <Link className="login-panel__back-btn" to="/">
              WSTECZ
            </Link>
            <button className="login-panel__submit-btn" type="button" onClick={handleSubmit}>
              ZALOGUJ
            </button>
          </div>

          <button className="login-panel__support-link" type="button" onClick={onOpenSupport}>
            Problemy z logowaniem
          </button>
        </div>
      </div>
    </div>
  );
}