﻿using FootballHub.Application.Interfaces;
using FootballHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.Infrastructure.Persistence;

public class MainDbContext: DbContext, IApplicationDbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountUser> AccountUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<decimal>().HavePrecision(18, 4);
        base.ConfigureConventions(configurationBuilder);
    }
}
