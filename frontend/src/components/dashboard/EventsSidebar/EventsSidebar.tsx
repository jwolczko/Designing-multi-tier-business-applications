import './EventsSidebar.css';

type EventItem = {
  text: string;
  amount: string;
  success?: boolean;
  positive?: boolean;
};

type EventGroup = {
  title: string;
  items: EventItem[];
};

const groups: EventGroup[] = [
  {
    title: 'Dziś',
    items: [{ text: 'Wydatki', amount: '117,27 PLN' }],
  },
  {
    title: '03 kwietnia 2026',
    items: [
      { text: 'Wydatki', amount: '1 356,78 PLN' },
      { text: 'Przelew na kwotę 1 284,04 PLN został zlecony.', amount: '', success: true },
    ],
  },
  {
    title: '02 kwietnia 2026',
    items: [
      { text: 'Wydatki', amount: '342,06 PLN' },
      { text: 'Wpływy', amount: '3,07 PLN', positive: true },
      { text: 'Wydatki', amount: '293,77 PLN' },
    ],
  },
  {
    title: '31 marca 2026',
    items: [
      { text: 'Wpływy', amount: '7,01 PLN', positive: true },
      { text: 'Wydatki', amount: '1,34 PLN' },
      { text: 'Transakcja pomiędzy kontami', amount: '2 000,00 PLN' },
      { text: 'Transakcja pomiędzy kontami', amount: '2 000,00 PLN' },
    ],
  },
];

export function EventsSidebar() {
  return (
    <section className="events-sidebar">
      <h2>Wydarzenia</h2>

      {groups.map((group) => (
        <div className="events-sidebar__group" key={group.title}>
          <h3>{group.title}</h3>
          <div className="events-sidebar__items">
            {group.items.map((item, index) => (
              <article
                className={`events-sidebar__item ${item.success ? 'events-sidebar__item--success' : ''}`}
                key={`${group.title}-${index}`}
              >
                <div className="events-sidebar__item-left">
                  <span className={`events-sidebar__icon ${item.positive ? 'events-sidebar__icon--positive' : ''}`}>
                    {item.positive ? '↪' : item.success ? '✓' : '⊞'}
                  </span>
                  <span className="events-sidebar__text">{item.text}</span>
                </div>
                {item.amount && (
                  <div className={`events-sidebar__amount ${item.positive ? 'events-sidebar__amount--positive' : ''}`}>
                    {item.amount} <span>▾</span>
                  </div>
                )}
              </article>
            ))}
          </div>
        </div>
      ))}
    </section>
  );
}
