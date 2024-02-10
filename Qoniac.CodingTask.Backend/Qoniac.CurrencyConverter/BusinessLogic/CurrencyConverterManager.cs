using Microsoft.Extensions.Logging;
using Qoniac.CodingTask.CurrencyConverter.Interfaces;
using Qoniac.CurrencyConverter.DTOs;

namespace Qoniac.CodingTask.CurrencyConverter.BusinessLogic
{
    /*
     * CurrencyConveterManager behaves as bridge between web server and actual implementation of conversion.
     * It performs first all sorts of validation ( e.g. Format and range )
     * followed by the actual conversion.
    */
    public class CurrencyConverterManager : ICurrencyConverterManager
    {
        public ICurrencyFormatValidator CurrencyFormatValidator { get; private set; }
        public ICurrencyRangeValidator CurrencyRangeValidator { get; private set; }
        public ICurrencyConverter CurrencyConverter { get; private set; }
        public ILogger<CurrencyConverterManager> _logger { get; private set; }
        public CurrencyConverterManager(ICurrencyFormatValidator currencyFormatValidator, ICurrencyRangeValidator currencyRangeValidator,
                                        ICurrencyConverter currencyConverter, ILogger<CurrencyConverterManager> logger)
        {
            _logger = logger;
            CurrencyRangeValidator = currencyRangeValidator;
            CurrencyConverter = currencyConverter;
            CurrencyFormatValidator = currencyFormatValidator;
        }

        public ConversionResult ConvertToWords(string currencyValue)
        {
            ConversionResult result;

            try
            {
                result = CurrencyFormatValidator.ValidateFormat(currencyValue);

                if (result.Success)
                {
                    result = CurrencyRangeValidator.ValidateRange(currencyValue);

                    if (result.Success)
                    {
                        return CurrencyConverter.ConvertCurrencyToWords(currencyValue);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured during CurrencyConverterManager.ConvertToWords. See the exception stackTrace for more details.");
                throw;
            }
        }
    }
}
