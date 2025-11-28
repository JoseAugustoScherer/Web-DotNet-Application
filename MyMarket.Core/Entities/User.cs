using System.Text.RegularExpressions;
using MyMarket.Core.Enums;

namespace MyMarket.Core.Entities;

public sealed class User : BaseEntity
{
    public User()
    {
        
    }
    
    public User(
        string name,
        string lastName,
        string email,
        decimal amount,
        string password,
        Gender gender,
        DateTime birthDate,
        Role role,
        ActiveStatus activeStatus)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Amount = amount;
        Password = password;
        Gender = gender;
        BirthDate = birthDate;
        Role = role;
        ActiveStatus = activeStatus;
    }
    
    public string Name { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public decimal Amount { get; private set; }
    public string Password { get; private set; }
    public Gender Gender { get; private set; }
    public DateTime BirthDate { get; private set; }
    public Role Role { get; private set; }
    public ActiveStatus ActiveStatus { get; set; } =  ActiveStatus.Active;


    public void UpdateName(string name)
    {
        IsValidateString(name);
        Name = name;
    }

    public void UpdateLastName(string lastName)
    {
        IsValidateString(lastName);
        LastName = lastName;
    }

    public void UpdateEmail(string email)
    {
        IsValidEmail(email);
        Email = email;
    }

    public void IncreaseAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException($"'{nameof(amount)}' cannot be negative.", nameof(amount));
        
        Amount += amount;
    }

    public void DecreaseAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException($"'{nameof(amount)}' cannot be negative.", nameof(amount));
        if (Amount - amount < 0)
            throw new ArgumentException($"'{nameof(amount)}' cannot be negative.", nameof(amount));
        
        Amount -= amount;
    }
    
    public void UpdatePassword(string password)
    {
        IsValidPassword(password);
        Password = password;
    }

    public void UpdateGender(Gender gender)
    {
        Gender = gender;
    }

    public void UpdateBirthDate(DateTime birthDate)
    {
        BirthDate = birthDate;
    }

    public void UpdateRole(Role role)
    {
        Role = role;
    }

    public void UpdateStatus(ActiveStatus activeStatus)
    {
        ActiveStatus = activeStatus;
    }
    
    private static void IsValidateString(string field)
    {
        if (string.IsNullOrWhiteSpace(field))
            throw new ArgumentException($"'{nameof(field)}' cannot be null or whitespace.", nameof(field));
    }

    private static void IsValidEmail(string email)
    {
        const string reGex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        if (!Regex.IsMatch(email, reGex))
            throw new ArgumentException($"'{nameof(email)}' is not a valid email address.");
    }

    private static void IsValidPassword(string password)
    {
        const string reGex = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9a-zA-Z$*&@#]{8,}$";
        if (!Regex.IsMatch(password, reGex))
            throw new ArgumentException($"'{nameof(password)}' is not a valid password.");
    }
}