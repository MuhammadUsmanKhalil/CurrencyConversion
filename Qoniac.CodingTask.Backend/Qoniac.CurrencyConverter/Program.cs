using CommandLine;
using Qoniac.CodingTask.CurrencyConverter.BusinessLogic;
using Qoniac.CurrencyConverter.DTOs;

var currencyConverterManager = new CurrencyConverterManager(new DollarCurrencyFormatValidator(),
                                                                    new DollarCurrencyRangeValidator(),
                                                                    new DollarCurrencyConverter(null), null);

var conversion = currencyConverterManager.ConvertToWords("-1114550,99");
if (conversion.Success)
{
    Console.WriteLine(conversion.Cast<ConversionResult<string>>().Data);
}
