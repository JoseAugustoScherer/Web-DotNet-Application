using System.Text.RegularExpressions;
using MyMarket.Core.Enums;

namespace MyMarket.Core.Entities;

public sealed class User(
    string name,
    string lastName,
    string email,
    string password,
    Gender gender,
    DateTime birthDate,
    Role role,
    ActiveStatus activeStatus,
    DateTime createdOn)
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public String Name { get; private set; } = name;
    public String LastName { get; private set; } = lastName;
    public String Email { get; private set; } = email;
    public String Password { get; private set; } = password;
    public Gender Gender { get; private set; } = gender;
    public DateTime BirthDate { get; private set; } = birthDate;
    public Role Role { get; private set; } = role;
    public ActiveStatus ActiveStatus { get; set; } =  ActiveStatus.Active;
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public DateTime ModifiedOn { get; private set; }

    public void UpdateName(string name)
    {
        IsValidateString(name);
        Name = name;
        ModifiedOn = DateTime.UtcNow;
    }

    public void UpdateLastName(string lastName)
    {
        IsValidateString(lastName);
        LastName = lastName;
        ModifiedOn = DateTime.UtcNow;
    }

    public void UpdateEmail(string email)
    {
        IsValidEmail(email);
        Email = email;
        ModifiedOn = DateTime.UtcNow;
    }

    public void UpdatePassword(string password)
    {
        IsValidPassword(password);
        Password = password;
        ModifiedOn = DateTime.UtcNow;
    }

    public void UpdateGender(Gender gender)
    {
        Gender = gender;
        ModifiedOn = DateTime.UtcNow;
    }

    public void UpdateBirthDate(DateTime birthDate)
    {
        BirthDate = birthDate;
        ModifiedOn = DateTime.UtcNow;
    }

    public void UpdateRole(Role role)
    {
        Role = role;
        ModifiedOn = DateTime.UtcNow;
    }

    public void UpdateStatus(ActiveStatus activeStatus)
    {
        ActiveStatus = activeStatus;
        ModifiedOn = DateTime.UtcNow;
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