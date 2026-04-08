import './LandingHeader.css';

type LandingHeaderProps = {
  onOpenLogin: () => void;
};

export function LandingHeader({ onOpenLogin }: LandingHeaderProps) {
  return (
    <header className="landing-header">
      <div className="landing-header__topbar">
        <nav className="landing-header__audiences">
          <a className="landing-header__audience landing-header__audience--active">KLIENCI INDYWIDUALNI</a>
        </nav>
      </div>

      <div className="landing-header__toolbar">
     
        <button className="landing-header__outline-btn" type="button">
          ZAŁÓŻ KONTO
        </button>
        <button className="landing-header__login-btn" type="button" onClick={onOpenLogin}>
          LOGOWANIE
        </button>
      </div>

      <nav className="landing-header__menu">
        <a>Konta</a>
        <a>Karty</a>
        <a>Kredyty</a>
        <a>Oszczędności</a>
        <a>Inwestycje</a>
        <a>Ubezpieczenia</a>
        <a>Bankowość elektroniczna</a>
        <a>Wsparcie</a>
        <a>Kontakt</a>
      </nav>
    </header>
  );
}
