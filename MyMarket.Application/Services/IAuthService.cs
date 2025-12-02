using MyMarket.Core.Enums;

namespace MyMarket.Application.Services;

public interface IAuthService
{
    string GenerateJwtToken(Guid userId, string email, string role);
    string ComputeSha256Hash(string password);
}