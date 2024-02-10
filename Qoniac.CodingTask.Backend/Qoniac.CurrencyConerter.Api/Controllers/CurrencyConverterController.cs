using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Qoniac.CodingTask.CurrencyConverter.Interfaces;
using Qoniac.CurrencyConverter.DTOs;

namespace Qoniac.CodingTask.Controllers
{
    /// <summary>
    /// Represents REST interface for currency conversion
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class CurrencyConverterController : ControllerBase
    {
        private readonly ILogger<CurrencyConverterController> _logger;
        private readonly IMemoryCache _currencyCache;
        private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;

        private readonly ICurrencyConverterManager _currencyConvertManager;

        public CurrencyConverterController(ILogger<CurrencyConverterController> logger, ICurrencyConverterManager currencyConvertManager,
                                           IMemoryCache currencyCache)
        {
            _currencyCache = currencyCache;
            _logger = logger;
            _currencyConvertManager = currencyConvertManager;

            _memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2) //can configure this from appSettings.json
            };
        }

        /// <summary>
        /// Converts the currency value to words.
        /// </summary>
        /// <param name="currencyValue">The currency value to convert.</param>
        /// <returns>The words representation of the currency.</returns>

        [HttpGet("convert-to-words", Name = nameof(ConvertToWords))]

        public ActionResult<ConversionResult> ConvertToWords([FromQuery] string currencyValue)
        {
            try
            {
                ConversionResult cachedResult;

                if (_currencyCache.TryGetValue(currencyValue, out cachedResult))
                {
                    return Ok(cachedResult);
                }

                var result = _currencyConvertManager.ConvertToWords(currencyValue);

                if (!result.Success)
                    return BadRequest(result.Exception);

                _currencyCache.Set(currencyValue, result, _memoryCacheEntryOptions);

                return Ok((ConversionResult<string>)result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the ConvertToWords request.");
                return BadRequest(ex.Message);
            }
        }
    }
}