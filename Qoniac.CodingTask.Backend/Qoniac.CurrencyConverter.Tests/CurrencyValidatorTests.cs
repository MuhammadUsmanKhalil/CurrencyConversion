using Qoniac.CodingTask.CurrencyConverter.BusinessLogic;
using Qoniac.CodingTask.CurrencyConverter.Interfaces;

namespace Qoniac.CurrencyConverterTests
{
    [TestClass]
    public class CurrencyValidatorTests
    {
        [TestMethod]
        [DataRow("999, 99", true)]
        [DataRow("-100, 99", true)]
        [DataRow("999999999", true)]
        [DataRow("999999999,-99", false)]
        [DataRow("1000000000,99", false)]
        [DataRow("-999999999,99", true)]
        [DataRow("999, -1", false)]
        [DataRow("999, 100", false)]

        public void MinMaxRangeValidationTests_Must_Pass(string currencyValue, bool expectedResult)
        {
            ICurrencyRangeValidator currencyRangeValidator = new DollarCurrencyRangeValidator();
            var results = currencyRangeValidator.ValidateRange(currencyValue);

            Assert.AreEqual(results.Success, expectedResult);
        }

        [TestMethod]
        [DataRow("88887...47,44", false)]
        [DataRow("1111,5558.78", false)]
        [DataRow(".........787,445", false)]
        [DataRow("4444,45555,455", false)]
        [DataRow("88.888.778", false)]
        [DataRow("88.888", false)]
        [DataRow("", false)]
        [DataRow("999 999 ABC", false)]
        [DataRow("999 999,ABC", false)]
        [DataRow("ABC DEF", false)]
        [DataRow("()!@#12458$,99", false)]
        [DataRow("9999999", true)]
        [DataRow("9999999,99", true)]
        [DataRow("0,01", true)]
        [DataRow("0", true)]
        [DataRow("1", true)]

        public void InvalidFormatTest_Must_Pass(string invalidFormatCurrency, bool expectedResult)
        {
            ICurrencyFormatValidator currencyFormatValidator = new DollarCurrencyFormatValidator();
            var results = currencyFormatValidator.ValidateFormat(invalidFormatCurrency);

            Assert.AreEqual(results.Success, expectedResult);
        }
    }
}