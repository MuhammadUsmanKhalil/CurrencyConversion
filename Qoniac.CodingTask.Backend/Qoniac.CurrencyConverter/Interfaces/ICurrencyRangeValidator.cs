
using Qoniac.CurrencyConverter.DTOs;

namespace Qoniac.CodingTask.CurrencyConverter.Interfaces
{
    public interface ICurrencyRangeValidator
    {
        ConversionResult ValidateRange(string value);
    }
}
