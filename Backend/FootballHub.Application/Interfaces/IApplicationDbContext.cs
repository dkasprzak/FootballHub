using FootballHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Application.Interfaces;

public interface IApplicationDbContext
{  
    DbSet<User> Users { get; set; }
    DbSet<Account> Accounts { get; set; }
    DbSet<AccountUser> AccountUsers { get; set; }
    DbSet<Country> Countries { get; set; }
    DbSet<League> Leagues { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
