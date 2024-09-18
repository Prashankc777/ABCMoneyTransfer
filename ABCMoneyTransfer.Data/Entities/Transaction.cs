using System;
using System.Collections.Generic;

namespace ABCMoneyTransfer.Data.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public decimal TransferAmountMyr { get; set; }

    public decimal ExchangeRate { get; set; }

    public DateTime CreatedDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public decimal Amount { get; set; }

    public string BankName { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}
