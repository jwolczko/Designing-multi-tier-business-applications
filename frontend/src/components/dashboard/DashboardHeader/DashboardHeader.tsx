import { useNavigate } from 'react-router-dom';
import './DashboardHeader.css';

export function DashboardHeader() {
  const navigate = useNavigate();

  return (
    <header className="dashboard-header">
      <div className="dashboard-header__left">
        <span className="dashboard-header__page-name">Strona Główna</span>
      </div>

      <div className="dashboard-header__right">
        <div className="dashboard-header__user">
          <div className="dashboard-header__avatar">◯</div>
          <span>Imię i Nazwisko</span>
          
        </div>

        <div className="dashboard-header__mail">✉<span className="dashboard-header__badge">13</span></div>

        <button className="dashboard-header__logout" type="button" onClick={() => navigate('/')}>
          <span>⏻</span>
          <div>
            <strong>WYLOGUJ</strong>
            <span>05:00</span>
          </div>
        </button>
      </div>
    </header>
  );
}