using Qoniac.CurrencyConverter.DTOs;

namespace Qoniac.CodingTask.CurrencyConverter.Interfaces
{
    public interface ICurrencyConverter
    {
        ConversionResult<string> ConvertCurrencyToWords(string currencyValue);
    }
}
