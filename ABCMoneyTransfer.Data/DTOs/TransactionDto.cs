using ABCMoneyTransfer.Data.Entities;

namespace ABCMoneyTransfer.Data.DTOs;

public class TransactionDto
{
    public int Id { get; set; }

    public string SenderGivenName { get; set; }

    public string ReceiverGivenName { get; set; }

    public decimal TransferAmountMyr { get; set; }

    public decimal ExchangeRate { get; set; }

    public DateTime CreatedDate { get; set; }
    public decimal Amount { get; set; }

    public string BankName { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

}

public class TransactionInsertDto
{
    public int Id { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public decimal TransferAmountMyr { get; set; }

    public decimal ExchangeRate { get; set; }

    public string BankName { get; set; } = null!;

    public string AccountNumber { get; set; } = null!;

    public decimal Amount => Math.Round(TransferAmountMyr * ExchangeRate, 2);
}