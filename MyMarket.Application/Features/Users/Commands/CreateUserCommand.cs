using MyMarket.Application.Abstractions;
using MyMarket.Core.Enums;

namespace MyMarket.Application.Features.Users.Commands;

public sealed record CreateUserCommand
{
    public string Name { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public decimal Amount { get; init; }
    public string Password { get; init; }
    public Gender Gender { get; init; }
    public DateTime BirthDate { get; init; }
    public Role Role { get; init; }
    public ActiveStatus ActiveStatus { get; init; }
    public DateTime CreatedOn { get; init; }
}