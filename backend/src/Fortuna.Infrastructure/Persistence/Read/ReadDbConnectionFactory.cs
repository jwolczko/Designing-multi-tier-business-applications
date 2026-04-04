using System.Data;
using Fortuna.Application.Abstractions.Persistence;
using Fortuna.Infrastructure.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Fortuna.Infrastructure.Persistence.Read;

public sealed class ReadDbConnectionFactory : IReadDbConnectionFactory
{
    private readonly DatabaseOptions _options;

    public ReadDbConnectionFactory(IOptions<DatabaseOptions> options)
    {
        _options = options.Value;
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_options.ReadConnectionString);
}
