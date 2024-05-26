using FootballHub.Application.Interfaces;

namespace FootballHub.Application.Logic.Abstractions;

public abstract class BaseQueryHandler
{
    protected readonly ICurrentAccountProvider _currentAccountProvider;
    protected readonly IApplicationDbContext _applicationDbContext;

    public BaseQueryHandler(ICurrentAccountProvider currentAccountProvider, IApplicationDbContext applicationDbContext)
    {
        _currentAccountProvider = currentAccountProvider;
        _applicationDbContext = applicationDbContext;
    }
}
