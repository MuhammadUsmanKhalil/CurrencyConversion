using Qoniac.CodingTask.CurrencyConverter.Interfaces;
using Qoniac.CurrencyConverter.DTOs;
using System.Text.RegularExpressions;

namespace Qoniac.CodingTask.CurrencyConverter.BusinessLogic
{    
    /*
     * Following are set of validation classes. 
     * Range Validator implements the maximum range for expected dollars and cents.
     * Format Validator implements the validation for the format of dollars and cents. (e.g. only real time format is accepted)
    */
    public class DollarCurrencyRangeValidator : ICurrencyRangeValidator
    {
        public const long _maxDollarsAmount = 999999999;
        public const long _maxCentsAmmount = 99;        

        public ConversionResult ValidateRange(string currencyValue)
        {
            var dollarCurrency = Dollar.ToDollar(currencyValue);

            if (dollarCurrency.PrimaryUnit > _maxDollarsAmount)
                return ConversionResult.Fail(new InvalidOperationException("Currency conversion failed ! Dollars exceeded the supported range. It should be less than or equal to 999,999,999."));

            if (dollarCurrency.FractionalUnit.HasValue && dollarCurrency.FractionalUnit.GetValueOrDefault() > _maxCentsAmmount || dollarCurrency.FractionalUnit.GetValueOrDefault() < 0)
                return ConversionResult.Fail(new InvalidOperationException("Currency conversion failed! Cents must be between 0-99."));

            return ConversionResult.Ok();
        }
    }

    public class DollarCurrencyFormatValidator : ICurrencyFormatValidator
    {
        private const string _supportedCurrencyRegexPattern = "^[0-9]*$";

        public ConversionResult ValidateFormat(string currencyValue)
        {
            if (string.IsNullOrEmpty(currencyValue))
                return ConversionResult.Fail(new InvalidOperationException("Invalid currency: Please provide a non-empty currency value!"));

            var completeCurrencyValue = currencyValue.Split(',');

            if (completeCurrencyValue.Length > 2)
                return ConversionResult.Fail(new InvalidOperationException("Invalid currency format: only currency (dollars,cents (e.g. 123 or 123,45)) notation is allowed!"));

            var dollarValue = Regex.Replace(completeCurrencyValue[0], @"\s", "");

            if (completeCurrencyValue.Length > 1 && completeCurrencyValue.Length <= 2)
            {
                var centsValue = Regex.Replace(completeCurrencyValue[1], @"\s", "");

                if (!new Regex(_supportedCurrencyRegexPattern).IsMatch(dollarValue))
                {
                    return ConversionResult.Fail(new InvalidOperationException("Invalid currency format: Dollars are in invalid format (Use a proper real number format)!"));
                }

                if (!new Regex(_supportedCurrencyRegexPattern).IsMatch(centsValue))
                {
                    return ConversionResult.Fail(new InvalidOperationException("Invalid currency fromat: Cents are in invalid format (Use a proper real number format)!"));
                }

                return ConversionResult.Ok();
            }
            else
            {
                if (!new Regex(_supportedCurrencyRegexPattern).IsMatch(dollarValue))
                {
                    return ConversionResult.Fail(new InvalidOperationException("Invalid currency format: Dollars are in invalid format (Use a proper real number format! Supported format (e.g. 123 or 123,45))!"));
                }

                return ConversionResult.Ok();
            }
        }
    }
}
