using MyMarket.Application.Abstractions;
using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Users.Commands;

public sealed record CreateUserCommand (
    string Name,
    string LastName,
    string Email,
    decimal Amount,
    string Password,
    Gender Gender,
    DateTime BirthDate,
    Role Role,
    ActiveStatus ActiveStatus);