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
                f => f.PickRandom(new string[] {"@Password12345", "@#$SomethingHere", "HelloI'm=password"}))
            .RuleFor(u => u.Gender,
                f => f.Random.Enum<Gender>())
            .RuleFor(u => u.BirthDate,
                f => new DateTime(f.Random.Int(2000, 2025)))
            .RuleFor(u => u.ActiveStatus,
                f => f.Random.Enum<ActiveStatus>());

        var users = userFaker.Generate(count);

        return users;
    }
}