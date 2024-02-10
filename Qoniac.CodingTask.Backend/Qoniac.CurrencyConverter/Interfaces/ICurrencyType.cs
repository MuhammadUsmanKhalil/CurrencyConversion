namespace Qoniac.CodingTask.CurrencyConverter.Interfaces
{
    public interface ICurrencyType<T> where T : IFormattable
    {
        public T PrimaryUnit { get; set; }
        public int? FractionalUnit { get; set; }
    }
}
