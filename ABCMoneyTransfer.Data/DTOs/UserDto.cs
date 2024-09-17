using ABCMoneyTransfer.Data.Entities;
using ABCMoneyTransfer.Data.Utilities;

namespace ABCMoneyTransfer.Data.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int CountryId { get; set; }
    public bool IsSender { get; set; }
    public string GivenName { get; set; } = null!;
    public string CountryName { get; set; } = null!;
}

public class UserInsertDto
{
    public string FirstName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int CountryId { get; set; }
    public bool IsSender { get; set; }
    public string GivenName => GeneralUtility.CombineName(FirstName, MiddleName, LastName);
}

public class UserUpdateDto : UserInsertDto
{
    public int Id { get; set; }
}

public class UserDeleteDto
{
    public int Id { get; set; }
}