import { DashboardHeader } from '../../components/dashboard/DashboardHeader/DashboardHeader';
import { SummarySection } from '../../components/dashboard/SummarySection/SummarySection';
import { ProductsSection } from '../../components/dashboard/ProductsSection/ProductsSection';
import { EventsSidebar } from '../../components/dashboard/EventsSidebar/EventsSidebar';
import './DashboardPage.css';

export function DashboardPage() {
  return (
    <div className="dashboard-page">
      <DashboardHeader />

      <div className="dashboard-page__content">
        <main className="dashboard-page__main">
          <SummarySection />
          <ProductsSection />
        </main>
        
        <aside className="dashboard-page__sidebar">
          <EventsSidebar />
        </aside>
      </div>
    </div>
  );
}