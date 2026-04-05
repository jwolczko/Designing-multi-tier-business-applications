import './LandingHero.css';

export function LandingHero() {
  return (
    <section className="landing-hero">
      <div className="landing-hero__grid">
        <div className="landing-hero__content">
          <h1>Spokojnych i ciepłych Świąt Wielkanocnych</h1>
          <p>Życzymy jak najwięcej radości, odpoczynku i dobrej pogody!</p>
        </div>

        <div className="landing-hero__visual">
          <div className="landing-hero__eggs">
            <div className="landing-hero__egg landing-hero__egg--pink">M</div>
            <div className="landing-hero__egg landing-hero__egg--mint" />
            <div className="landing-hero__egg landing-hero__egg--light" />
            <div className="landing-hero__egg landing-hero__egg--rose" />
          </div>
        </div>
      </div>

      <div className="landing-hero__cards">
        <div className="landing-hero__card landing-hero__card--dark">
          <strong>ŚWIĄTECZNE ŻYCZENIA</strong>
          <span>›</span>
        </div>
        <div className="landing-hero__card">
          <strong>POŻYCZKA GOTÓWKOWA (RRSO 8,3%)</strong>
          <p>Promocja do 15.05.2026</p>
        </div>
        <div className="landing-hero__card">
          <strong>KREDYT HIPOTECZNY (RRSO 6,49%)</strong>
          <p>Znajdź swoje miejsce</p>
        </div>
        <div className="landing-hero__card">
          <strong>ZWROTY ZA ZAKUPY</strong>
          <p>Przemyślany sposób na zakupy</p>
        </div>
      </div>
    </section>
  );
}