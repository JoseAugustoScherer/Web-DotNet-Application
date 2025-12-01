using Bogus;
using MyMarket.Core.Entities;
using MyMarket.Core.Enums;

namespace MyMarketTest.Utils.UserTest;

public static class FakeDataUser
{
    public static List<User> FakeUserList(int count)
    {
        var userFaker = new Faker<User>("en")
            .RuleFor(u => u.Id,
                f => f.Random.Guid())
            .RuleFor(u => u.Name,
                f => f.Name.FirstName(Bogus.DataSets.Name.Gender.Female))
            .RuleFor(u => u.LastName,
                f => f.Name.LastName(Bogus.DataSets.Name.Gender.Female))
            .RuleFor(u => u.Email,
                f => f.Internet.Email(f.Person.FirstName).ToLower())
            .RuleFor(u => u.Amount,
                f => f.Random.Decimal(500, 2000))
            .RuleFor(u => u.Password,
                f => f.PickRandom(new string[] {"@Password12345", "@#$SomethingHere11", "HelloI'm=p4ssword"}))
            .RuleFor(u => u.Gender,
                f => f.Random.Enum<Gender>())
            .RuleFor(u => u.BirthDate, 
                f => f.Date.Between(new DateTime(2000, 1, 1), new DateTime(2025, 12, 31)))
            .RuleFor(f => f.Role,
                f => f.Random.Enum<Role>())
            .RuleFor(u => u.ActiveStatus,
                f => f.Random.Enum<ActiveStatus>());

        var users = userFaker.Generate(count);

        return users;
    }
}