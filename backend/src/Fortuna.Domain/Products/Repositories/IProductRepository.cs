using Fortuna.Domain.Products;

namespace Fortuna.Domain.Products.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
}
