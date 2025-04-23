using Core.Utils.Enums;

namespace Core.Currency.Components.Events
{
    public struct MoneyCurrencyChangedRequestComponent
    {
        public OperationType Operation;
        public double Value;
    }
}
