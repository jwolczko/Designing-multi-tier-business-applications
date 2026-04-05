import './ProductsSection.css';

const products = [
  {
    title: 'Konto 360°',
    subtitle: '69 6969 1313 6969 1313 6576 6969',
    label: 'Dostępne środki',
    amount: '10 000 000,70 PLN',
    accent: '',
  },
  {
    title: 'Konto Oszczędnościowe Sknerus',
    subtitle: '13 6969 1313 0000 1313 6969 1313',
    label: 'Dostępne środki',
    amount: '20 000 000 000 000 000,00 PLN',
    accent: '',
  },
  {
    title: 'Visa Impresja',
    subtitle: 'KREDYTOWA GŁÓWNA',
    label: 'Dostępne środki',
    amount: '3 000 000,00 PLN',
    accent: 'z 3 000 000,00 PLN',
  },
];

export function ProductsSection() {
  return (
    <section className="products-section">
      <div className="products-section__header">
        <h2>Moje produkty</h2>
      </div>

      <div className="products-section__grid">
        {products.map((product) => (
          <article className="products-section__card" key={product.title}>
            <div className="products-section__card-header">
              <h3>{product.title}</h3>
              <button type="button">⋮</button>
            </div>

            <div className="products-section__subtitle">{product.subtitle}</div>
            <div className="products-section__label">{product.label}</div>
            <div className="products-section__amount">{product.amount}</div>
            {product.accent && <div className="products-section__accent">{product.accent}</div>}
          </article>
        ))}

        <article className="products-section__add-card">
          <div className="products-section__plus">＋</div>
          <h3>Dodaj nowy produkt</h3>
          <p>do swojej bankowości</p>
        </article>
      </div>
    </section>
  );
}