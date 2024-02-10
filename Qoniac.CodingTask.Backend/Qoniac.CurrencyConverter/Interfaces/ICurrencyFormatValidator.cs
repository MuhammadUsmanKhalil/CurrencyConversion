using Qoniac.CurrencyConverter.DTOs;

namespace Qoniac.CodingTask.CurrencyConverter.Interfaces
{
    public interface ICurrencyFormatValidator
    {
        ConversionResult ValidateFormat(string currencyValue);
    }
}
