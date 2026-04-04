using System.Data;

namespace Fortuna.Application.Abstractions.Persistence;

public interface IReadDbConnectionFactory
{
    IDbConnection CreateConnection();
}
