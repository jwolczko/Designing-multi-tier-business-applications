using System.Data;
using System.Text.Json;
using Fortuna.Domain.Accounts.Events;
using Fortuna.Infrastructure.Persistence.Write;
using Fortuna.Infrastructure.Projections.Interfaces;
using Fortuna.Infrastructure.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Fortuna.Infrastructure.Projections;

public sealed class ProjectionDispatcher : IReadModelProjector
{
    private readonly string _connectionString;

    public ProjectionDispatcher(IOptions<DatabaseOptions> options)
    {
        _connectionString = options.Value.ReadConnectionString;
    }

    public async Task ProjectAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

   
}
