using System.ComponentModel.DataAnnotations;

namespace Fitradar.Domain
{
    public sealed record Money
    {
        public decimal? Amount { get; set; }

        [MaxLength(3)]
        public string Currency { get; set; }

        public Money(decimal? amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public Money(Money moneyCopy)
        {
            Amount = moneyCopy.Amount;
            Currency = moneyCopy.Currency;
        }

        public Money(long amount, string currency)
        {
            Amount = amount / 100.0m;
            Currency = currency;
        }
    }
}