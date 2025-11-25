using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Users.Queries;

public sealed record UserDto(
    Guid Id,
    string Name,
    string LastName,
    string Email,
    string Password,
    Gender Gender,
    DateTime BirthDate,
    Role Role,
    ActiveStatus ActiveStatus,
    DateTime CreatedOn
    );