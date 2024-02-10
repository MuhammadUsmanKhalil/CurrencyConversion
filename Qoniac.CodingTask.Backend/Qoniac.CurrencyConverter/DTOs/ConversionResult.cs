namespace Qoniac.CurrencyConverter.DTOs
{
    /*
     * Generic Result classes whcih holds the success/failure status of the operation. 
     * It also holds the exception and data (in case if data is available)
    */
    public class ConversionResult
    {
        public bool Success { get; }
        public Exception? Exception { get; }

        public ConversionResult(bool success, Exception? exception)
        {
            Success = success;
            Exception = exception;
        }

        public static ConversionResult Ok()
        {
            return new ConversionResult(true, null);
        }

        public static ConversionResult Fail(Exception exception)
        {
            return new ConversionResult(false, exception);
        }
    }

    public class ConversionResult<T> : ConversionResult
    {
        public T Data { get; }

        public ConversionResult(bool success, Exception? exception, T data) : base(success, exception)
        {
            Data = data;
        }
    }
}
