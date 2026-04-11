import type { DashboardData } from '../../../features/dashboard/types/dashboard.types';
import {
  getProductAmountLabel,
  getProductCategoryLabel,
  getProductDisplayName,
  getProductSubtitle,
  getProductTypeLabel,
} from '../../../features/dashboard/productPresentation';
import './ProductsSection.css';

type ProductsSectionProps = {
  dashboard: DashboardData;
};

function formatMoney(amount: number, currency: string) {
  return new Intl.NumberFormat('pl-PL', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(amount) + ` ${currency}`;
}

export function ProductsSection({ dashboard }: ProductsSectionProps) {
  return (
    <section className="products-section">
      <div className="products-section__header">
        <h2>Moje produkty</h2>
      </div>

      <div className="products-section__grid">
        {dashboard.products.map((product) => (
          <article className="products-section__card" key={product.productId}>
            <div className="products-section__card-header">
              <h3>{getProductDisplayName(product.productName)}</h3>
              <button type="button">⋮</button>
            </div>

            <div className="products-section__subtitle">{getProductSubtitle(product)}</div>
            <div className="products-section__label">
              {getProductCategoryLabel(product.productCategory)} • {getProductTypeLabel(product.productType)}
            </div>
            <div className="products-section__label">{getProductAmountLabel(product)}</div>
            <div className="products-section__amount">{formatMoney(product.balance, product.currency)}</div>
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
