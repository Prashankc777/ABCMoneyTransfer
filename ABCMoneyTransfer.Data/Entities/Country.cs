using System;
using System.Collections.Generic;

namespace ABCMoneyTransfer.Data.Entities;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
