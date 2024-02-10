
using Qoniac.CurrencyConverter.DTOs;

namespace Qoniac.CodingTask.CurrencyConverter.Interfaces
{
    public interface ICurrencyConverterManager
    {
        ConversionResult ConvertToWords(string currencyValue);
    }
}
