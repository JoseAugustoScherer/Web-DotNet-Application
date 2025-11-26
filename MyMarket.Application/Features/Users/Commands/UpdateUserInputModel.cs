using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Users.Commands;

public sealed record UpdateUserInputModel(
    string FirstName,
    string LastName,
    string Email,
    decimal Amount,
    string Password,
    Gender? Gender,
    DateTime? BirthDate,
    Role Role,
    ActiveStatus ActiveStatus
    );