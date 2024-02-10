
using Microsoft.Extensions.Logging;
using Qoniac.CodingTask.CurrencyConverter.Interfaces;
using Qoniac.CurrencyConverter.DTOs;
using System.Text.RegularExpressions;

namespace Qoniac.CodingTask.CurrencyConverter.BusinessLogic
{
    /*
     * Dollar class behaves as only specific to deal dollars and cents, where dollars should be within in the specified range of integer. ( In this example the maximum dollar size : 999,999,999)
     * Cents are also limited. So, it's data type would also be considered as int.
     * 
     * For any other currency, (e.g. Euro, AED, PND, Kr), new currency type can be written with desired number of digits by respecting the interface of ICurrencyType. In that case, PrimaryUnit can 
     * of long, or any other numeric data type. 
    */
    public class Dollar : ICurrencyType<int>
    {
        public string Symbol { get; private set; }
        public int PrimaryUnit { get; set; }
        public int? FractionalUnit { get; set; }

        private Dollar(int dollar, int? cents)
        {
            Symbol = "$";

            PrimaryUnit = dollar;
            FractionalUnit = cents;
        }

        public static Dollar ToDollar(string currencyValue)
        {
            try
            {
                var completeCurrencyValue = Regex.Replace(currencyValue, @"\s", "").Split(',');

                if (completeCurrencyValue.Length > 1 && completeCurrencyValue.Length <= 2)
                {
                    return new Dollar(int.Parse(completeCurrencyValue[0]), int.Parse(completeCurrencyValue[1]));
                }
                else
                    return new Dollar(int.Parse(completeCurrencyValue[0]), null);
            }
            catch(OverflowException)
            {
                throw new InvalidOperationException("Currency conversion failed ! Supported range for dollar between 0-999,999,999 and cents between 0-99. ");
            }
            catch(Exception)
            {
                throw;
            }
        }
    }


    /* 
     * DollarCurrencyConverter converts the number into english words.
     * The whole process consists of the following steps.
     *   - It splits the complete number into multiple chunks of three-digits, where first chunk pos would be hundred, second would be thousand and last would be of million.
     *   - Each chunk is devided by 100. The result and remainder contributed individually.
     *   - The result and remainders indexed from arrays where multiple of ten or numbers less than 20 placed. 
     *   - Each chunk is provided it's position, which translated into it's corresponding unit (i.e. Hundred, Thousand or Million ) and placed accordingly.  
    */
    public class DollarCurrencyConverter : ICurrencyConverter
    {
        public const int _thousand = 1000;
        public const int _hundred = 100;

        private const string _HundredWord = "hundred";
        private const string _HundredThousandWord = "hundred thousand";
        private const string _HundredMillionWord = "hundred million";
        private const string _ThousandWord = "thousand";
        private const string _MillionWord = "million";

        private static string[] _lessThanTwentyWordsArray = new string[]
        {
            "",
            "one","two", "three", "four", "five", "six",
            "seven","eight","nine", "ten","eleven","twelve",
            "thirteen","fourteen","fifteen","sixteen",
            "seventeen","eighteen","nineteen"
        };

        private static string[] _tensArray = new string[] { "", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

        private enum UnitWords
        {
            Hundred = 0,
            Thousand,
            Million
        };

        private ILogger<DollarCurrencyConverter> _logger;

        public DollarCurrencyConverter(ILogger<DollarCurrencyConverter> logger)
        {
            _logger = logger;
        }


        private static readonly Func<int, string, string> _lessThanTwentyWordsWithHundredUnit = (num, hundredMultipleWord) => _lessThanTwentyWordsArray != null ? _lessThanTwentyWordsArray[num / 100] + " " + hundredMultipleWord : "";
        private static readonly Func<int, int, string, string> _remainderWords = (num, rem, hundredMultipleWord) =>
        {
            return rem < 20 && _lessThanTwentyWordsArray != null ? _lessThanTwentyWordsArray[rem] + (!string.IsNullOrEmpty(hundredMultipleWord) ? " " + hundredMultipleWord : "")
                                                                 : _tensArray != null && _lessThanTwentyWordsArray != null ?
                                                                                _tensArray[rem / 10] + (num % 10 != 0 ? "-" + _lessThanTwentyWordsArray[num % 10] : "")
                                                                                + (!string.IsNullOrEmpty(hundredMultipleWord) ? " " + hundredMultipleWord : "") : "";
        };

        private static string GetWordsByChunk(UnitWords chunkPos, int rem, int num)
        {
            switch (chunkPos)
            {
                case UnitWords.Hundred:

                    if (num / _hundred > 0)
                    {
                        return rem == 0 ? _lessThanTwentyWordsWithHundredUnit(num, _HundredWord) : _lessThanTwentyWordsWithHundredUnit(num, _HundredWord) + " " + _remainderWords(num, rem, "");
                    }
                    else
                    {
                        return rem != 0 ? _remainderWords(num, rem, "") : "";
                    }

                case UnitWords.Thousand:

                    if (num / _hundred > 0)
                    {
                        return rem == 0 ? _lessThanTwentyWordsWithHundredUnit(num, _HundredThousandWord) : _lessThanTwentyWordsWithHundredUnit(num, _HundredWord) + " " +
                                                                                                           _remainderWords(num, rem, _ThousandWord);
                    }
                    else
                    {
                        return rem != 0 ? _remainderWords(num, rem, _ThousandWord) : "";
                    }

                case UnitWords.Million:

                    if (num / _hundred > 0)
                    {
                        return rem == 0 ? _lessThanTwentyWordsWithHundredUnit(num, _HundredMillionWord) : _lessThanTwentyWordsWithHundredUnit(num, _HundredWord) + " " +
                                                                                                          _remainderWords(num, rem, _MillionWord);
                    }
                    else
                    {
                        return rem != 0 ? _remainderWords(num, rem, _MillionWord) : "";
                    }
            }

            return "";
        }

        private static string ToEnglishWords(int inputCurrency)
        {
            string englishWords = "";

            var dollars = inputCurrency;
            var chunks = CreateChunks(dollars);

            int chunkCount = 0;

            foreach (var chunk in chunks)
            {
                var chunkWords = GetWordsByChunk((UnitWords)chunkCount, chunk % _hundred, chunk);

                if (!string.IsNullOrEmpty(chunkWords))
                {
                    englishWords = chunkWords + " " + englishWords;
                }

                chunkCount++;
            }

            return englishWords.Trim();
        }

        private static List<int> CreateChunks(int dollars)
        {
            List<int> chunks = new();

            while (dollars > 0)
            {
                var remainder = dollars % _thousand;
                dollars /= _thousand;

                chunks.Add(remainder);
            }

            return chunks;
        }

        /*
         * ConvertCurrencyToWords can be implemented to implement conversion from source currency to target representation. ( e.g. in this case in English Words.)         
        */

        public ConversionResult<string> ConvertCurrencyToWords(string inputCurrency)
        {
            try
            {
                string englishWords = "";

                if (!string.IsNullOrEmpty(inputCurrency))
                {
                    var dollarInputCurrency = Dollar.ToDollar(inputCurrency);

                    if (dollarInputCurrency.PrimaryUnit == 0 && (!dollarInputCurrency.FractionalUnit.HasValue || dollarInputCurrency.FractionalUnit == 0))
                        return new ConversionResult<string>(true, null, "zero dollars");

                    if (dollarInputCurrency.PrimaryUnit == 0 && dollarInputCurrency.FractionalUnit.HasValue)
                    {
                        englishWords = "zero dollars and " + ToEnglishWords(dollarInputCurrency.FractionalUnit.Value) + (dollarInputCurrency.FractionalUnit.Value > 1 ? " cents" : " cent");
                        return new ConversionResult<string>(true, null, englishWords);
                    }

                    if (dollarInputCurrency.PrimaryUnit < 0)
                    {
                        dollarInputCurrency.PrimaryUnit = -1 * dollarInputCurrency.PrimaryUnit;
                        englishWords = "negative ";
                    }

                    englishWords += $"{ToEnglishWords(dollarInputCurrency.PrimaryUnit)}{(dollarInputCurrency.PrimaryUnit > 1 ? " dollars" : " dollar")}" +
                    $"{(dollarInputCurrency.FractionalUnit.GetValueOrDefault() != 0 ? " and " + ToEnglishWords(dollarInputCurrency.FractionalUnit.GetValueOrDefault()) + (dollarInputCurrency.FractionalUnit.GetValueOrDefault() > 1 ? " cents" : " cent") : "")}";

                    return new ConversionResult<string>(true, null, englishWords);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during the DollarCurrencyConverter.ConvertCurrencyToWords. See the exception stackTrace for more details.");
                throw;
            }

            return new ConversionResult<string>(false, new InvalidOperationException("Currency conversion failed!"), "");
        }
    }
}
