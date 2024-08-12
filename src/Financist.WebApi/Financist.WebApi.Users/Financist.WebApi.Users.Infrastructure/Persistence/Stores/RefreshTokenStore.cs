using Financist.WebApi.Shared.System;
using Financist.WebApi.Users.Application.Abstractions.Authentication;
using Financist.WebApi.Users.Application.Abstractions.Persistence;
using Financist.WebApi.Users.Application.Abstractions.Persistence.Stores;
using Financist.WebApi.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financist.WebApi.Users.Infrastructure.Persistence.Stores;

public class RefreshTokenStore : IRefreshTokenStore
{
    private readonly IUsersDbContext _dbContext;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;

    public RefreshTokenStore(
        IUsersDbContext dbContext,
        IRefreshTokenGenerator refreshTokenGenerator,
        IDateTimeOffsetProvider dateTimeOffsetProvider)
    {
        _dbContext = dbContext;
        _refreshTokenGenerator = refreshTokenGenerator;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
    }

    public async Task<bool> IsValidAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return await _dbContext.RefreshTokens.AnyAsync(token =>
            token.Value == refreshToken && _dateTimeOffsetProvider.UtcNow <= token.ValidUntil, cancellationToken);
    }

    public async Task<RefreshTokenEntity> GetAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return await _dbContext.RefreshTokens.AsNoTracking()
            .SingleAsync(token => token.Value == refreshToken, cancellationToken);
    }
    
    public async Task<string> GenerateTokenAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _dbContext.RefreshTokens.Where(refreshToken => refreshToken.SubjectId == userId)
            .ExecuteDeleteAsync(cancellationToken);
        
        var token = _refreshTokenGenerator.GenerateRefreshToken();
        var generatedOn = _dateTimeOffsetProvider.UtcNow;
        var entity = new RefreshTokenEntity
        {
            Value = token,
            SubjectId = userId,
            GeneratedOn = generatedOn,
            ValidUntil = generatedOn.AddSeconds(_refreshTokenGenerator.Expiration),
        };

        await _dbContext.RefreshTokens.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return token;
    }

    public async Task InvalidateAsync(string refreshToken, CancellationToken cancellationToken)
    {
        await _dbContext.RefreshTokens.Where(token => token.Value == refreshToken)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
