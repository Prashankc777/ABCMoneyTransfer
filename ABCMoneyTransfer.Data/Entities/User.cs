using System;
using System.Collections.Generic;

namespace ABCMoneyTransfer.Data.Entities;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int CountryId { get; set; }

    public bool IsSender { get; set; }

    public string GivenName { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Transaction> TransactionReceivers { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionSenders { get; set; } = new List<Transaction>();
}
