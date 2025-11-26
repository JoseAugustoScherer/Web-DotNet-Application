using MyMarket.Core.Entities;
using MyMarket.Core.Enums;

namespace MyMarketTest.UserUnitTest;

public class CreateUserUnitTest
{
    [Theory]
    [InlineData("Juca", "Bala", "jucabala@gmail.com", 299.90,"@Senha123456", Gender.Male, "2000-05-12 00:00:00", Role.Guest, ActiveStatus.Active)]
    [InlineData("Maria", "Dolorida", "mariadolorida@gmail.com", 1280.00,"@Senha123456", Gender.Female, "1982-05-12 00:00:00", Role.Administrator, ActiveStatus.Active)]
    [InlineData("Clara", "Dark", "claradark@gmail.com", 300.00, "@Senha123456", Gender.Female, "2000-05-12 00:00:00", Role.Moderator, ActiveStatus.Active)]
    public void CreateProductTest(
        string userName,
        string userLastName,
        string userEmail,
        decimal userAmount,
        string userPassword,
        Gender userGender,
        DateTime userBirthDate,
        Role userRole,
        ActiveStatus userActiveStatus)
    {
        var user = new User(
            userName, 
            userLastName, 
            userEmail, 
            userAmount,
            userPassword, 
            userGender, 
            userBirthDate,
            userRole,
            userActiveStatus);
    
        Assert.NotNull(user);
        Assert.Equal(userName, user.Name);
        Assert.Equal(userLastName, user.LastName);
        Assert.Equal(userEmail, user.Email);
        Assert.Equal(userAmount, user.Amount);
        Assert.Equal(userPassword, user.Password);
        Assert.Equal(userGender, user.Gender);
        Assert.Equal(userBirthDate, user.BirthDate);
        Assert.Equal(userRole, user.Role);
        Assert.Equal(userActiveStatus, user.ActiveStatus);
    }
}