namespace Core.Currency.Interfaces
{
    public interface IMoneyChangedListener
    {
        public void OnMoneyChanged(double value);
    }
}
