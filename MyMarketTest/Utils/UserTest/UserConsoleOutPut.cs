using MyMarket.Core.Entities;

namespace MyMarketTest.Utils.UserTest;

// If you wanna see what kind of data the Bogus lib uses to create/update the fake user

public class UserConsoleOutPut
{
    public void ConsoleOutput(User user)
    {
        Console.WriteLine("\n\n=== FAKE DATA OF FAKE PRODUCT ===");
        Console.WriteLine($"ID: {user.Id}");
        Console.WriteLine($"Name: {user.Name}");
        Console.WriteLine($"LastName: {user.LastName}");
        Console.WriteLine($"Email: {user.Email}");
        Console.WriteLine($"Amount: {user.Amount:C}");
        Console.WriteLine($"Password: {user.Password}");
        Console.WriteLine($"Gender: {user.Gender}");
        Console.WriteLine($"BirthDate: {user.BirthDate}");
        Console.WriteLine($"Role: {user.Role}");
        Console.WriteLine($"ActiveStatus: {user.ActiveStatus}");
        Console.WriteLine("=================================\n\n");
    }
}