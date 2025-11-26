namespace MyMarket.Application.Abstractions;

public interface IAuthService
{
    string GenerateJwtToken(Guid userId, string email, int role);
    string ComputeSha256Hash(string password);
}