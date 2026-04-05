import './SummarySection.css';

export function SummarySection() {
  return (
    <section className="summary-section">
      <div className="summary-section__header-row">
        <h2>Podsumowanie środków</h2>
      </div>

      <div className="summary-section__top-grid">
        <div className="summary-section__left">
          <div className="summary-section__stack summary-section__stack--back-1" />
          <div className="summary-section__stack summary-section__stack--back-2" />

          <div className="summary-section__card">
            <div className="summary-section__card-header">
              <h3>Konto bogatego programisty</h3>
            </div>

            <div className="summary-section__amount">
              10 000 000,70 PLN
            </div>

            <div className="summary-section__actions">
              <button type="button" className="summary-section__primary-btn">
                WYKONAJ PRZELEW
              </button>
              <button type="button" className="summary-section__outline-btn">
                HISTORIA
              </button>
            </div>
          </div>
        </div>

        
        <div className="summary-section__right">         
        </div>
      </div>
    </section>
  );
}