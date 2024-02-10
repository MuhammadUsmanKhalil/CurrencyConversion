using Microsoft.Extensions.Logging;
using Qoniac.CodingTask.CurrencyConverter.BusinessLogic;
using Qoniac.CodingTask.CurrencyConverter.Interfaces;

namespace Qoniac.CurrencyConverterTests
{
    public class FakeLogger : ILogger<DollarCurrencyConverter>
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class CurrencyConversionTests
    {
        [TestMethod]

        [DataRow("0", "zero dollars")]
        [DataRow("1", "one dollar")]
        [DataRow("2", "two dollars")]
        [DataRow("9", "nine dollars")]

        [DataRow("-9", "negative nine dollars")]
        [DataRow("-9, 9", "negative nine dollars and nine cents")]
        [DataRow("11, 9", "eleven dollars and nine cents")]
        [DataRow("20, 99", "twenty dollars and ninety-nine cents")]
        [DataRow("30, 99", "thirty dollars and ninety-nine cents")]
        [DataRow("40, 99", "forty dollars and ninety-nine cents")]
        [DataRow("50, 99", "fifty dollars and ninety-nine cents")]
        [DataRow("60, 99", "sixty dollars and ninety-nine cents")]
        [DataRow("70, 99", "seventy dollars and ninety-nine cents")]
        [DataRow("80, 99", "eighty dollars and ninety-nine cents")]
        [DataRow("90, 99", "ninety dollars and ninety-nine cents")]
        [DataRow("100, 99", "one hundred dollars and ninety-nine cents")]
        [DataRow("995, 99", "nine hundred ninety-five dollars and ninety-nine cents")]
        [DataRow("1001, 99", "one thousand one dollars and ninety-nine cents")]
        [DataRow("9999, 99", "nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]
        [DataRow("10000, 99", "ten thousand dollars and ninety-nine cents")]
        [DataRow("11000, 99", "eleven thousand dollars and ninety-nine cents")]
        [DataRow("50000, 99", "fifty thousand dollars and ninety-nine cents")]
        [DataRow("99000, 99", "ninety-nine thousand dollars and ninety-nine cents")]
        [DataRow("100000, 99", "one hundred thousand dollars and ninety-nine cents")]
        [DataRow("100001, 99", "one hundred thousand one dollars and ninety-nine cents")]
        [DataRow("1000000, 99", "one million dollars and ninety-nine cents")]
        [DataRow("1055000, 99", "one million fifty-five thousand dollars and ninety-nine cents")]
        [DataRow("10550000, 99", "ten million five hundred fifty thousand dollars and ninety-nine cents")]
        [DataRow("305550000, 99", "three hundred five million five hundred fifty thousand dollars and ninety-nine cents")]
        [DataRow("999550000, 99", "nine hundred ninety-nine million five hundred fifty thousand dollars and ninety-nine cents")]
        [DataRow("999999999, 99", "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]
        [DataRow("999999999", "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars")]


        public void CurrencyNumbers_Must_match_expected_output_in_english_words(string currencyValue, string expectedOutput)
        {
            ICurrencyConverter currencyConverter = new DollarCurrencyConverter(new FakeLogger());
            var results = currencyConverter.ConvertCurrencyToWords(currencyValue);

            Assert.IsTrue(results.Success);
            Assert.AreEqual(results.Data, expectedOutput);
        }
    }
}