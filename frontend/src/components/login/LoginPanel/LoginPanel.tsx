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
            Hasło <span className="login-panel__hint">i</span>
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

          <div className="login-panel__info-blocks">
            <div>
              <h3>Fałszywi konsultanci</h3>
              <p>
                Oszuści podszywają się pod pracowników Banku - nie pobieraj nieznanych aplikacji i nie
                udostępniaj poufnych danych <span>Więcej</span>
              </p>
            </div>

            <div>
              <h3>Finanse 360°</h3>
              <p>
                Sprawdź saldo i zlecaj przelewy z kont w innych bankach wygodnie przez Millenet <span>Więcej</span>
              </p>
            </div>
          </div>
        </div>
      </div>

      <div className="login-panel__right">
        <div className="login-panel__footer-links">
          <span>🔒 Bezpieczeństwo</span>
          <span>🛡 Oddziały i bankomaty</span>
          <span>✉ Napisz do nas</span>
        </div>
      </div>
    </div>
  );
}