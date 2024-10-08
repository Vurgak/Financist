﻿using Financist.WebApi.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financist.WebApi.Users.Application.Abstractions.Persistence;

public interface IUsersDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<RefreshTokenEntity> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
