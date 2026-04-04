using Fortuna.Application.Dashboard.Queries.GetDashboard;
using Fortuna.ReadModel.Dashboard.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Fortuna.ReadModel.DependencyInjection;

public static class ReadModelServiceRegistration
{
    public static IServiceCollection AddReadModel(this IServiceCollection services)
    {
        services.AddScoped<IDashboardReadRepository, DashboardReadRepository>();
        return services;
    }
}
