using Microsoft.EntityFrameworkCore;
using UserContacts.Core.Errors;
using UserContactsManager.Dal;
using UserContactsManager.Dal.Entities;

namespace UserContactsManager.Bll.Repository.RepositoryService;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly MainContext _mainContext;
    public async Task AddRefreshToken(RefreshToken refreshToken)
    {
        await _mainContext.RefreshTokens.AddAsync(refreshToken);
        await _mainContext.SaveChangesAsync();
    }

    public async Task DeleteRefreshToken(string refreshToken)
    {
        var token = _mainContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        if (token == null)
        {
            throw new EntityNotFoundException();
        }
    }

    public async Task<RefreshToken> SelectRefreshToken(string refreshToken, long userId) => await _mainContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId);
}

